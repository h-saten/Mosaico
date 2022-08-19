using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Payments.Binance.Abstractions;
using Mosaico.Payments.Binance.Models;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.ConfirmBinanceTransactionJob, IsRecurring = true, Cron = "*/3 * * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ConfirmBinanceTransactionJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;
        private readonly IBinanceClient _binanceClient;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        
        public ConfirmBinanceTransactionJob(IWalletDbContext walletDbContext, ILogger logger, IBinanceClient binanceClient, IProjectManagementClient projectManagementClient, ICrowdsalePurchaseService crowdsalePurchaseService, IEventFactory eventFactory, IEventPublisher eventPublisher, ICrowdsaleDispatcher crowdsaleDispatcher)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _binanceClient = binanceClient;
            _projectManagementClient = projectManagementClient;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _crowdsaleDispatcher = crowdsaleDispatcher;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var transactions = await _walletDbContext.Transactions.Where(t => t.PaymentProcessor == Domain.ProjectManagement.Constants.PaymentMethods.Binance &&
                                                                              t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
            _logger?.Information($"Starting scan of Binance transactions. Found {transactions.Count} transactions to check");
            var statuses = await _walletDbContext.TransactionStatuses.ToListAsync();
            if (!statuses.Any())
            {
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
            }
            foreach (var transaction in transactions)
            {
                _logger?.Verbose($"Checking transaction {transaction.CorrelationId}");
                var binanceResponse = await _binanceClient.GetOrderAsync(new BinanceOrderRequest
                {
                    PrepayId = transaction.ExtraData
                });
                if (binanceResponse != null)
                {
                    try
                    {
                        if (binanceResponse.Status.ToUpper() == Payments.Binance.Constants.TransactionStatuses.SUCCEEDED)
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} succeeded in Binance");
                            transaction.FinishedAt = DateTimeOffset.UtcNow;
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed));
                            
                            if (transactions.Any(t =>
                                t.Id != transaction.Id && t.ExtraData == transaction.ExtraData &&
                                t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed))
                            {
                                _walletDbContext.Transactions.Remove(transaction);
                                await _walletDbContext.SaveChangesAsync();
                                continue;
                            }
                            var currentStage = await _projectManagementClient.GetStageAsync(transaction.StageId.Value);
                            transaction.TokenAmount = transaction.PayedInUSD / currentStage.TokenPrice;
                            await _crowdsalePurchaseService.AddTokenToUserWalletAsync(transaction.TokenId.Value, transaction.To, transaction.Network);
                            var transactionHash = await _crowdsalePurchaseService.TransferTargetTokensToUserAsync(transaction);
                            transaction.TransactionHash = transactionHash;
                            await _walletDbContext.SaveChangesAsync();
                            _logger?.Information($"Transaction {transaction.CorrelationId} succeeded in Mosaico");
                            await _crowdsaleDispatcher.PurchaseSuccessful(transaction.UserId, transaction.TransactionHash);
                            await PublishSuccessEventAsync(transaction);
                            await PublishStatisticEventsAsync(transaction);
                        }
                        else if (binanceResponse.Status.ToUpper() == Payments.Binance.Constants.TransactionStatuses.CANCELLED)
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} is cancelled in Binance");
                            transaction.FinishedAt = DateTimeOffset.UtcNow;
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Canceled));
                            await _walletDbContext.SaveChangesAsync();
                            await _crowdsaleDispatcher.PurchaseFailed(transaction.UserId, "Payment was cancelled");
                            await PublishFailEventAsync(transaction);
                        }
                        else if (binanceResponse.Status.ToUpper() == Payments.Binance.Constants.TransactionStatuses.FAILED)
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} failed in Binance");
                            transaction.FinishedAt = DateTimeOffset.UtcNow;
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Failed));
                            transaction.FailureReason = "Payment processor failure";
                            await _walletDbContext.SaveChangesAsync();
                            await _crowdsaleDispatcher.PurchaseFailed(transaction.UserId, "Payment failed");
                            await PublishFailEventAsync(transaction);
                        }
                        else if (binanceResponse.Status.ToUpper() ==
                                 Payments.Binance.Constants.TransactionStatuses.EXPIRED)
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} expired in Binance");
                            transaction.FinishedAt = DateTimeOffset.UtcNow;
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Expired));
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
                _logger?.Verbose($"Binance returned empty response for transaction {transaction.CorrelationId}");
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