using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Payments.RampNetwork.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.RampTransactionConfirmationJob, IsRecurring = true, Cron = "*/5 * * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ConfirmRampTransactionJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IRampClient _rampClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IFeeService _feeService;

        public ConfirmRampTransactionJob(IWalletDbContext walletDbContext, ILogger logger, ICrowdsaleDispatcher crowdsaleDispatcher, ICrowdsalePurchaseService crowdsalePurchaseService, IEventPublisher eventPublisher, IEventFactory eventFactory, IRampClient rampClient, IExchangeRateService exchangeRateService, IProjectManagementClient projectManagementClient, IFeeService feeService)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _crowdsaleDispatcher = crowdsaleDispatcher;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _rampClient = rampClient;
            _exchangeRateService = exchangeRateService;
            _projectManagementClient = projectManagementClient;
            _feeService = feeService;
        }
    
        public override async Task ExecuteAsync(object parameters = null)
        {
            var transactions = await _walletDbContext.Transactions.Where(t => t.PaymentProcessor == Payments.RampNetwork.Constants.PaymentProcessorName && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
            _logger?.Information($"Starting scan of RAMP transactions. Found {transactions.Count} transactions to check");
            var statuses = await _walletDbContext.TransactionStatuses.ToListAsync();
            if (!statuses.Any())
            {
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
            }
            var rates = await _exchangeRateService.GetExchangeRatesAsync();
            foreach (var transaction in transactions)
            {
                _logger?.Verbose($"Checking transaction {transaction.CorrelationId}");
                var rampResponse = await _rampClient.GetPurchaseAsync(transaction.CorrelationId, transaction.ExtraData);
                if (rampResponse != null)
                {
                    try
                    {
                        if (rampResponse.Status.ToUpper() == Payments.RampNetwork.Constants.TransactionStatuses.SUCCEEDED)
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} succeeded in RAMP");
                            transaction.FinishedAt = rampResponse.UpdatedAt;
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed));
                            transaction.ExchangeTransactionHash = rampResponse.FinalTxHash;
                            transaction.Fee = rampResponse.Fee;
                            var rate = rates.FirstOrDefault(r => r.Ticker == transaction.Currency);
                            transaction.PayedInUSD = rampResponse.FiatValue * rate.Rate;
                            transaction.FeeInUSD = rampResponse.Fee * rate.Rate;
                            transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, rampResponse.Fee, rampResponse.FiatValue);
                            transaction.MosaicoFeeInUSD = transaction.MosaicoFee * rate.Rate;
                            
                            if (transactions.Any(t =>
                                t.Id != transaction.Id && t.CorrelationId == transaction.CorrelationId &&
                                t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed))
                            {
                                _walletDbContext.Transactions.Remove(transaction);
                                await _walletDbContext.SaveChangesAsync();
                                continue;
                            }
                            
                            if (rampResponse.ReceiverAddress.ToLower() == transaction.IntermediateAddress.ToLower())
                            {
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
                            else
                            {
                                _logger?.Information($"Transaction {transaction.CorrelationId} has invalid receiver address");
                                transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Failed));
                                transaction.FailureReason = "Invalid receiver wallet";
                                await _walletDbContext.SaveChangesAsync();
                                await _crowdsaleDispatcher.PurchaseFailed(transaction.UserId, "Payment failed");
                                await PublishFailEventAsync(transaction);
                            }
                        }
                        else if (rampResponse.Status.ToUpper() == Payments.RampNetwork.Constants.TransactionStatuses.CANCELLED)
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} is cancelled in RAMP");
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Canceled));
                            await _walletDbContext.SaveChangesAsync();
                            await _crowdsaleDispatcher.PurchaseFailed(transaction.UserId, "Payment was cancelled");
                            await PublishFailEventAsync(transaction);
                        }
                        else if (rampResponse.Status.ToUpper() == Payments.RampNetwork.Constants.TransactionStatuses.FAILED)
                        {
                            _logger?.Information($"Transaction {transaction.CorrelationId} failed in RAMP");
                            transaction.FinishedAt = rampResponse.UpdatedAt;
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Failed));
                            transaction.FailureReason = "Payment processor failure";
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
                _logger?.Verbose($"Ramp returned empty response for transaction {transaction.CorrelationId}");
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