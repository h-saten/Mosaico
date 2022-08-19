using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Commands.Transactions.InitiateNativeCurrencyPurchaseTransaction;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Services
{
    public class BuyTokenService : IBuyTokenService
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly ICrowdsaleService _crowdSaleService;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        private readonly IDateTimeProvider _provider;
        private readonly IIndex<string, IPaymentTokenService> _paymentTokensServices;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;

        public BuyTokenService(
            IWalletDbContext walletDbContext, 
            IProjectManagementClient projectManagementClient, 
            ICrowdsaleService crowdSaleService, 
            ICrowdsaleDispatcher crowdsaleDispatcher, 
            IDateTimeProvider provider, 
            IIndex<string, IPaymentTokenService> paymentTokensServices, 
            IEventFactory eventFactory, 
            IEventPublisher eventPublisher,
            ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _projectManagementClient = projectManagementClient;
            _crowdSaleService = crowdSaleService;
            _crowdsaleDispatcher = crowdsaleDispatcher;
            _provider = provider;
            _paymentTokensServices = paymentTokensServices;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }
        
        public async Task BuyAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            await using var dbTransaction = _walletDbContext.BeginTransaction();
            string userId = default;
            try
            {
                _logger?.Verbose($"Received proper {nameof(TransactionInitiatedEvent)}");
                var transaction =
                    await _walletDbContext.Transactions.FirstOrDefaultAsync(t =>
                        t.Id == transactionId, cancellationToken);

                if (transaction == null || !transaction.TokenAmount.HasValue)
                {
                    throw new TransactionNotFoundException(transactionId.ToString());
                }
                
                _logger?.Verbose($"Transaction {transaction.Id} was found");
                
                userId = transaction.UserId;
                
                if (transaction.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase
                    && transaction.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending)
                {
                    var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId, cancellationToken);
                    if (token == null)
                    {
                        throw new TokenNotFoundException(transaction.TokenId.ToString());
                    }

                    _logger?.Verbose($"Token {token.Id} was fetched");

                    var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                        w.Network == token.Network
                        && w.UserId == transaction.UserId
                        && w.AccountAddress == transaction.From, cancellationToken);

                    if (wallet == null)
                    {
                        throw new WalletNotFoundException(transaction.UserId, token.Network);
                    }
                    _logger?.Verbose($"Wallet {wallet.Id} was fetched");

                    if (!transaction.StageId.HasValue)
                    {
                        throw new StageNotFoundException(Guid.Empty);
                    }
                    
                    var stage = await _projectManagementClient.GetStageAsync(transaction.StageId.Value, cancellationToken);
                    if (stage == null)
                    {
                        throw new StageNotFoundException(transaction.StageId.Value);
                    }
                    
                    var projectId = stage.ProjectId;
                    var project = await _projectManagementClient.GetProjectDetailsAsync(projectId, cancellationToken);
                    if (project == null)
                    {
                        throw new ProjectNotFoundException(projectId);
                    }
                    
                    _logger?.Verbose($"Project {projectId} details data was fetched");

                    var exchangeRate = await GetExchangeRateAsync(transaction.PaymentCurrency.Ticker, stage.TokenPrice);
                    var paymentCurrencyAmount = transaction.TokenAmount.Value * exchangeRate;
                    var transactionHash = string.Empty;
                    if (transaction.PaymentCurrency.NativeChainCurrency is false)
                    {
                        _logger?.Verbose($"Transaction of exchange stable coin '{project.CrowdsaleContractAddress}' ({wallet.Network}) started.");

                        var paymentTokensServiceKey = transaction.Currency;
                        if (!_paymentTokensServices.TryGetValue(paymentTokensServiceKey, out var paymentTokenService))
                        {
                            throw new UnknownContractVersionException("ERC20 Payment Currency", paymentTokensServiceKey);
                        }
                        
                        await paymentTokenService.SetWalletAllowanceAsync(token.Network, x =>
                        {
                            x.Amount = paymentCurrencyAmount;
                            x.ContractAddress = transaction.PaymentCurrency.ContractAddress;
                            x.OwnerPrivateKey = wallet.PrivateKey;
                            x.SpenderAddress = project.CrowdsaleContractAddress;
                            x.Decimals = transaction.PaymentCurrency.DecimalPlaces;
                            x.OwnerAddress = wallet.AccountAddress;
                        });
                        transactionHash = await _crowdSaleService.ExchangeTokens(token.Network, project.CrowdsaleContractAddress, p =>
                        {
                            p.PaymentTokenAddress = transaction.PaymentCurrency.ContractAddress;
                            p.Amount = paymentCurrencyAmount;
                            p.Beneficiary = wallet.AccountAddress;
                            p.SenderPrivateKey = wallet.PrivateKey;
                            p.PaymentTokenDecimalPlaces = transaction.PaymentCurrency.DecimalPlaces;
                        }); 
                    }
                    else
                    {
                        _logger?.Verbose($"Transaction of exchange native currency ({transaction.PaymentCurrency.Ticker}) started.");
                        transactionHash = await _crowdSaleService.BuyTokens(token.Network, project.CrowdsaleContractAddress, configuration =>
                        {
                            configuration.Amount = paymentCurrencyAmount;
                            configuration.SenderPrivateKey = wallet.PrivateKey;
                            configuration.Beneficiary = wallet.AccountAddress;
                        });
                    }
                    await _crowdsaleDispatcher.PurchaseSuccessful(transaction.UserId, transactionHash);
                    
                    transaction.SetStatus(await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(ts =>
                        ts.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed, cancellationToken: cancellationToken));
                    transaction.FinishedAt = _provider.Now();
                    transaction.TransactionHash = transactionHash;
                    transaction.PayedAmount = paymentCurrencyAmount;

                    if (!wallet.Tokens.Any(wt => wt.TokenId == token.Id))
                    {
                        wallet.Tokens.Add(new WalletToToken
                        {
                            Token = token,
                            TokenId = token.Id,
                            Wallet = wallet,
                            WalletId = wallet.Id
                        });
                        
                    }

                    await PublishEventsAsync(transaction, cancellationToken);
                    await _walletDbContext.SaveChangesAsync(cancellationToken);
                    await dbTransaction.CommitAsync(cancellationToken);
                }
                else
                {
                    _logger?.Warning($"Transaction {transaction.Id} is in wrong status to be confirmed");
                }
            }
            catch (Exception exception)
            {
                await dbTransaction.RollbackAsync(cancellationToken);
                await HandleTransactionFailure(transactionId, exception);
                await _crowdsaleDispatcher.PurchaseFailed(userId, exception.Message);
            }
        }

        private async Task HandleTransactionFailure(Guid transactionId, Exception exception)
        {
            var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
            if (transaction != null)
            {
                _walletDbContext.Transactions.Remove(transaction);
                await _walletDbContext.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetExchangeRateAsync(string ticker, decimal tokenPrice)
        {
            var exchangeRate = await _walletDbContext.ExchangeRates
                .OrderByDescending(er => er.CreatedAt)
                .Where(t => t.Ticker == ticker).Select(t => t.Rate)
                .FirstOrDefaultAsync();
            if (exchangeRate > 0 && tokenPrice > 0)
            {
                return tokenPrice / exchangeRate;
            }

            return 0;
        }
        
        private async Task PublishEventsAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new PurchaseTransactionConfirmedEvent
                {
                    Currency = transaction.Currency,
                    TransactionId = transaction.Id,
                    Payed = transaction.PayedAmount.Value,
                    TokensAmount = transaction.TokenAmount.Value,
                    TokenId = transaction.TokenId.Value,
                    ProjectId = transaction.ProjectId.Value,
                    RefCode = transaction.RefCode,
                    TransactionCorrelationId = transaction.CorrelationId,
                    UserId = transaction.UserId
                });
            await _eventPublisher.PublishAsync(cloudEvent.Source, cloudEvent, cancellationToken);
        }
    }
}