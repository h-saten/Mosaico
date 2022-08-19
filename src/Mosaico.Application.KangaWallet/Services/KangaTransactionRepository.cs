using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.KangaWallet.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;
using Constants = Mosaico.Domain.Wallet.Constants;

namespace Mosaico.Application.KangaWallet.Services
{
    public class KangaTransactionRepository : IKangaTransactionRepository
    {
        
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IKangaBuyApiClient _kangaBuyApiClient;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserManagementClient _userManagementClient;
        private readonly IDateTimeProvider _provider;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        
        public KangaTransactionRepository(
            IKangaBuyApiClient kangaBuyApiClient, 
            IWalletDbContext walletDbContext, 
            IUserManagementClient userManagementClient, 
            IEventFactory eventFactory, 
            IEventPublisher eventPublisher, 
            IDateTimeProvider provider, 
            IProjectManagementClient projectManagementClient, 
            IExchangeRateRepository exchangeRateRepository, 
            ILogger logger = null) 
        {
            _kangaBuyApiClient = kangaBuyApiClient;
            _walletDbContext = walletDbContext;
            _userManagementClient = userManagementClient;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _provider = provider;
            _projectManagementClient = projectManagementClient;
            _exchangeRateRepository = exchangeRateRepository;
            _logger = logger;
        }
        
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        [DisableConcurrentExecution(10 * 60)]
        public async Task SaveAsync(string transactionId)
        {
            _logger?.Information($"Start SaveTransactionCommand: {transactionId}");
            
            _logger?.Information($"Start saving transaction: '{transactionId}' from Kanga.");
            
            TransactionResponseDto transactionApiResponse;
    
            try
            {
                transactionApiResponse = await _kangaBuyApiClient.GetTransaction(transactionId);
            }
            catch (Exception)
            {
                _logger?.Error($"Error while get transaction: {transactionId}");
                throw new KangaException("get_transaction_operation_failed");
            }
            
            if (transactionApiResponse == null)
            {
                _logger?.Error($"Error while get transaction: {transactionId}. Data null.");
                throw new KangaException("get_transaction_data_null");
            }
            
            var token = await _walletDbContext
                .Tokens
                .AsNoTracking()
                .Where(m => m.Symbol.ToLower() == transactionApiResponse.Token.ToLower())
                .SingleOrDefaultAsync();
            
            if (token is null)
            {
                _logger?.Error($"Token: {transactionApiResponse.Token} from transaction: '{transactionId}' not found.");
                throw new TransactionTokenNotFoundException(transactionApiResponse.Token);
            }

            var transactionUser = await _userManagementClient.GetUserByEmailAsync(transactionApiResponse.Email);
            var transactionUserId = transactionUser?.Id;

            if (transactionUser is null)
            {
                transactionUserId =
                    await _userManagementClient.CreateExternalAccountAsync(transactionApiResponse.Email);
            }

            var internalTransaction = await _walletDbContext
                .Transactions
                .Where(x => x.Currency == transactionApiResponse.Currency
                            && x.TokenId == token.Id
                            && x.TokenAmount == transactionApiResponse.Quantity
                            && x.UserId == transactionUserId
                            && x.CreatedAt >= _provider.Now().AddDays(-7))
                .SingleOrDefaultAsync();

            if (internalTransaction is not null)
            {
                internalTransaction.PayedAmount = transactionApiResponse.Amount;
                internalTransaction.CorrelationId = transactionId;
            }
            else
            {
                var type = await _walletDbContext.TransactionType.FirstOrDefaultAsync(t => t.Key == Constants.TransactionType.Purchase);
                var projectResult = await _projectManagementClient.GetProjectWithActiveSale(token.Id);
                internalTransaction = new Transaction
                {
                    CorrelationId = transactionId,
                    UserId = transactionUserId,
                    TokenAmount = transactionApiResponse.Quantity,
                    TokenId = token.Id,
                    PaymentProcessor = "KANGA_EXCHANGE",
                    InitiatedAt = DateTimeOffset.UtcNow,
                    WalletAddress = null,
                    Network = token.Network,
                    PaymentCurrencyId = null,
                    PayedAmount = transactionApiResponse.Amount,
                    Currency = new KangaPaymentCurrency(transactionApiResponse.Currency).GlobalCurrencySymbol(),
                    ProjectId = projectResult?.Project.Id,
                    StageId = projectResult?.ActiveStageId
                };
                internalTransaction.SetType(type);

                _walletDbContext.Transactions.Add(internalTransaction);
            }

            if (internalTransaction.Currency != "USD")
            {
                var currencyExchangeRate = await _exchangeRateRepository
                    .GetExchangeRateAsync(internalTransaction.Currency);
                internalTransaction.PayedInUSD = internalTransaction.PayedAmount * currencyExchangeRate; 
            }
            
            var status = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                t.Key == Constants.TransactionStatuses.Confirmed);
            internalTransaction.SetStatus(status);
            
            await _walletDbContext.SaveChangesAsync();
            
            await PublishEventsAsync(internalTransaction, token.Id, internalTransaction.Id);
        }
        
        private async Task PublishEventsAsync(Transaction transaction, Guid tokenId, Guid transactionId)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new PurchaseTransactionConfirmedEvent
                {
                    TransactionId = transactionId,
                    Currency = transaction.Currency,
                    Payed = transaction.PayedAmount.Value,
                    TokensAmount = transaction.TokenAmount.Value,
                    TokenId = transaction.TokenId.Value,
                    ProjectId = transaction.ProjectId.Value,
                    RefCode = transaction.RefCode,
                    TransactionCorrelationId = transaction.CorrelationId,
                    UserId = transaction.UserId
                });
            await _eventPublisher.PublishAsync(cloudEvent.Source, cloudEvent);
        }
    }
}