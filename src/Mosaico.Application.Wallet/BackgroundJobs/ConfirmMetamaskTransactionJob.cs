using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Blockchain.Base.Extensions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.MetamaskConfirmationJob, IsRecurring = true, Cron = "*/5 * * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ConfirmMetamaskTransactionJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IFeeService _feeService;

        public ConfirmMetamaskTransactionJob(IWalletDbContext walletDbContext, ILogger logger, ICrowdsaleDispatcher crowdsaleDispatcher, ICrowdsalePurchaseService crowdsalePurchaseService, IEventPublisher eventPublisher, IEventFactory eventFactory, IEthereumClientFactory ethereumClientFactory, IDateTimeProvider dateTimeProvider, IFeeService feeService)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _crowdsaleDispatcher = crowdsaleDispatcher;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _ethereumClientFactory = ethereumClientFactory;
            _dateTimeProvider = dateTimeProvider;
            _feeService = feeService;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var transactions = await _walletDbContext.Transactions.Where(t => t.PaymentProcessor == Domain.ProjectManagement.Constants.PaymentMethods.Metamask &&
                                                            t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
            
            var statuses = await _walletDbContext.TransactionStatuses.ToListAsync();
            if (!statuses.Any())
            {
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
            }
            

            
            foreach (var transaction in transactions)
            {
                try
                {
                    var client = _ethereumClientFactory.GetClient(transaction.Network);
                    var tx = await client.GetTransactionAsync(transaction.ExchangeTransactionHash);
                    
                    if (tx?.Status == 1)
                    {
                        transaction.FinishedAt = _dateTimeProvider.Now();
                        transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed));
                        transaction.Fee = 0;
                        transaction.PayedInUSD = transaction.PayedAmount * transaction.ExchangeRate;
                        transaction.FeeInUSD = 0;
                        transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, transaction.Fee, transaction.PayedAmount.Value);
                        transaction.MosaicoFeeInUSD = transaction.MosaicoFee * transaction.ExchangeRate;
                        if (transactions.Any(t =>
                            t.Id != transaction.Id && t.CorrelationId == transaction.CorrelationId &&
                            t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed))
                        {
                            _walletDbContext.Transactions.Remove(transaction);
                            await _walletDbContext.SaveChangesAsync();
                            continue;
                        }
                        
                        transaction.TokenAmount = transaction.PayedInUSD / transaction.TokenPrice;
                        await _crowdsalePurchaseService.AddTokenToUserWalletAsync(transaction.TokenId.Value, transaction.To, transaction.Network);
                        var transactionHash = await _crowdsalePurchaseService.TransferTargetTokensToUserAsync(transaction);
                        transaction.TransactionHash = transactionHash;
                        await _walletDbContext.SaveChangesAsync();
                        await _crowdsaleDispatcher.PurchaseSuccessful(transaction.UserId, transaction.TransactionHash);
                        await PublishSuccessEventAsync(transaction);
                        await PublishStatisticEventsAsync(transaction);
                    }
                    else if(tx != null)
                    {
                        transaction.FinishedAt = _dateTimeProvider.Now();
                        transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Failed));
                        transaction.FailureReason = "Chain network failed";
                        await _walletDbContext.SaveChangesAsync();
                        await _crowdsaleDispatcher.PurchaseFailed(transaction.UserId, "Payment failed");
                        await PublishFailEventAsync(transaction);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warning($"{ex.Message} / {ex.StackTrace}");
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
                    ProjectId = transaction.ProjectId.Value,
                    TransactionCorrelationId = transaction.CorrelationId,
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