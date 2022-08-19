using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Blockchain.Base.Extensions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.ScanPurchaseOperationsJob, IsRecurring = true, Cron = "*/5 * * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ScanPurchaseOperationsJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IOperationService _operationService;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        private readonly ITokenLockService _lockService;
        private readonly IFeeService _feeService;
        private readonly ILogger _logger;

        public ScanPurchaseOperationsJob(IWalletDbContext walletDbContext, IEthereumClientFactory ethereumClientFactory, ILogger logger, IOperationService operationService, IDateTimeProvider timeProvider, ICrowdsaleDispatcher crowdsaleDispatcher, IEventFactory eventFactory, IEventPublisher eventPublisher, IFeeService feeService, ITokenLockService lockService)
        {
            _walletDbContext = walletDbContext;
            _ethereumClientFactory = ethereumClientFactory;
            _logger = logger;
            _operationService = operationService;
            _timeProvider = timeProvider;
            _crowdsaleDispatcher = crowdsaleDispatcher;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _feeService = feeService;
            _lockService = lockService;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            _logger.Information($"Starting scanning blockchain operations");
            var operations = await _walletDbContext.Operations.Where(o => o.Type == BlockchainOperationType.PURCHASE && 
                                                                          o.State == OperationState.IN_PROGRESS && o.TransactionHash != null).ToListAsync();
            _logger.Information($"Found {operations.Count} operations to scan");
            var statuses = await _walletDbContext.TransactionStatuses.ToListAsync();
            foreach (var operation in operations)
            {
                _logger.Verbose($"Starting to scan {operation.Id}");
                
                var client = _ethereumClientFactory.GetClient(operation.Network);
                var purchaseTransaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == operation.TransactionId);
                if(purchaseTransaction == null) continue;
                
                var transaction = await client.GetTransactionAsync(operation.TransactionHash);
                if (transaction.Status == 1)
                {
                    await _operationService.SetTransactionOperationCompleted(operation.Id, transaction.Gas.ConvertToDecimal());
                    await AcceptTransactionAsync(purchaseTransaction, operation, statuses);
                }
                else if(transaction.Status == 0)
                {
                    await _operationService.SetTransactionOperationFailed(operation.Id, "Transaction has invalid status");
                    await PublishFailEventAsync(purchaseTransaction);
                }
            }
            _logger?.Information($"Finished to analyze operations");
        }

        private async Task AcceptTransactionAsync(Transaction transaction, Operation operation, List<TransactionStatus> statuses)
        {
            if (transaction != null)
            {
                transaction.FinishedAt = _timeProvider.Now();
                transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed));
                transaction.PayedInUSD = transaction.PayedAmount * transaction.ExchangeRate;
                transaction.Fee = 0;
                transaction.FeeInUSD = 0;
                transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, transaction.Fee, transaction.PayedAmount.Value);
                transaction.MosaicoFeeInUSD = transaction.MosaicoFee * transaction.ExchangeRate;
                _walletDbContext.Transactions.Update(transaction);
                await _walletDbContext.SaveChangesAsync();
                await _crowdsaleDispatcher.PurchaseSuccessful(transaction.UserId, transaction.TransactionHash);
                if (transaction.UserId.ToLowerInvariant() != operation.UserId.ToLowerInvariant())
                {
                    await _crowdsaleDispatcher.PurchaseSuccessful(operation.UserId, transaction.TransactionHash);
                }

                await PublishSuccessEventAsync(transaction);
                await PublishStatisticEventsAsync(transaction);
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