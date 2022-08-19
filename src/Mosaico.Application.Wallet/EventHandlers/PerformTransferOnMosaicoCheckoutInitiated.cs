using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(PerformNonCustodialTransferOnInitiated), "wallets:api")]
    [EventTypeFilter(typeof(MosaicoWalletPurchaseInitiated))]
    public class PerformTransferOnMosaicoCheckoutInitiated : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly ITransactionService _transactionService;
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IOperationService _operationService;
        private readonly IEventPublisher _eventPublisher;
        
        public PerformTransferOnMosaicoCheckoutInitiated(IWalletDbContext walletDbContext, ICrowdsaleDispatcher crowdsaleDispatcher, 
            ICrowdsalePurchaseService crowdsalePurchaseService, ITransactionService transactionService, ILogger logger, IEventFactory eventFactory, IEventPublisher eventPublisher, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _crowdsaleDispatcher = crowdsaleDispatcher;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _transactionService = transactionService;
            _logger = logger;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _operationService = operationService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<MosaicoWalletPurchaseInitiated>();
            if(data != null)
            {
                var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == data.TransactionId);
                var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == data.OperationId);
                if(transaction != null && transaction.PaymentCurrency != null && transaction.PayedAmount > 0 && 
                   transaction.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending && operation != null) 
                {
                    try
                    {
                        _logger?.Information($"Transaction {transaction.CorrelationId} is ready to be processed");
                        var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                            w.UserId == transaction.UserId &&
                            w.AccountAddress == transaction.WalletAddress && transaction.Network == w.Network);
                        if (wallet == null)
                        {
                            throw new WalletNotFoundException(transaction.WalletAddress);
                        }

                        if (string.IsNullOrWhiteSpace(transaction.ExchangeTransactionHash))
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} executing exchange");
                            transaction.ExchangeTransactionHash =
                                await _crowdsalePurchaseService.WithdrawPaymentAsync(wallet, transaction,
                                    transaction.PayedAmount.Value);
                            
                            await _walletDbContext.SaveChangesAsync();
                        }

                        if (string.IsNullOrWhiteSpace(transaction.TransactionHash))
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} is executing token transfer");
                            var transactionHash = await _crowdsalePurchaseService.TransferTargetTokensToUserAsync(transaction);
                            await _transactionService.ConfirmTransactionAsync(transaction, transactionHash);
                            await _crowdsaleDispatcher.PurchaseSuccessful(data.UserId, transactionHash);
                            await _crowdsalePurchaseService.AddTokenToUserWalletAsync(transaction.TokenId.Value,
                                transaction.WalletAddress, transaction.Network);
                            await PublishStatisticEventsAsync(transaction);
                            await PublishSuccessEventAsync(transaction);
                            await _operationService.SetTransactionOperationCompleted(operation.Id, hash: transactionHash);
                        }
                    }
                    catch (Exception ex)
                    {
                        await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                        _logger?.Error(ex, "Error during mosaico transaction");
                        await _crowdsaleDispatcher.PurchaseFailed(data.UserId, ex.Message);
                        await _transactionService.FailTransactionAsync(transaction, ex.Message);
                        await PublishFailEventAsync(transaction);
                    }
                }
            }
        }
        
        private async Task PublishStatisticEventsAsync(Transaction transaction)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new PurchaseTransactionConfirmedEvent
                {
                    Currency = transaction.Currency,
                    TransactionId = transaction.Id,
                    Payed = transaction.PayedAmount.Value,
                    TokensAmount = transaction.TokenAmount.Value,
                    TokenId = transaction.TokenId.Value,
                    TransactionCorrelationId = transaction.CorrelationId,
                    ProjectId = transaction.ProjectId.Value,
                    RefCode = transaction.RefCode,
                    UserId = transaction.UserId
                });
            await _eventPublisher.PublishAsync(cloudEvent.Source, cloudEvent);
        }
        
        private async Task PublishSuccessEventAsync(Transaction transaction)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new SuccessfulPurchaseEvent(transaction.UserId, transaction.TokenAmount.Value,
                    transaction.StageId.Value, transaction.Id, transaction.PaymentProcessor));
            await _eventPublisher.PublishAsync(e);
        }
        
        private async Task PublishFailEventAsync(Transaction transaction)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new PurchaseFailedEvent(transaction.UserId, transaction.TokenAmount.Value,
                    transaction.StageId.Value, transaction.Id));
            await _eventPublisher.PublishAsync(e);
        }
    }
}