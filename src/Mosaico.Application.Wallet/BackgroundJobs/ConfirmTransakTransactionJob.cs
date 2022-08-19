using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Payments.Transak.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.TransakConfirmationJob, IsRecurring = true, Cron = "*/5 * * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ConfirmTransakTransactionJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ITransakClient _transakClient;
        private readonly IDateTimeProvider _provider;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IFeeService _feeService;
        private readonly IExchangeRateService _exchangeRateService;
        public readonly IOperationService _operationService;

        public ConfirmTransakTransactionJob(ILogger logger, IWalletDbContext walletDbContext, ICrowdsaleDispatcher crowdsaleDispatcher, ICrowdsalePurchaseService crowdsalePurchaseService, IEventPublisher eventPublisher, IEventFactory eventFactory, ITransakClient transakClient, IDateTimeProvider provider, IProjectManagementClient projectManagementClient, IFeeService feeService, IExchangeRateService exchangeRateService, IOperationService operationService)
        {
            _logger = logger;
            _walletDbContext = walletDbContext;
            _crowdsaleDispatcher = crowdsaleDispatcher;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _transakClient = transakClient;
            _provider = provider;
            _projectManagementClient = projectManagementClient;
            _feeService = feeService;
            _exchangeRateService = exchangeRateService;
            _operationService = operationService;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var transactions = await _walletDbContext.Transactions.Where(t => t.PaymentProcessor == Payments.Transak.Constants.PaymentProcessorName &&
                                                                              t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
            
            var statuses = await _walletDbContext.TransactionStatuses.ToListAsync();
            if (!statuses.Any())
            {
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
            }
            var rates = await _exchangeRateService.GetExchangeRatesAsync();
            foreach (var transaction in transactions)
            {
                var transakResponse = await _transakClient.GetOrderDetailsAsync(transaction.CorrelationId);
                if (transakResponse?.Response != null)
                {
                    try
                    {
                        if (transakResponse.Response.Status.ToUpper() == Payments.Transak.Constants.TransactionStatuses.SUCCEEDED)
                        {
                            transaction.FinishedAt = _provider.Now();
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed));
                            transaction.ExchangeTransactionHash = transakResponse.Response.TransactionHash;
                            transaction.Fee = transakResponse.Response.TotalFeeInFiat;
                            transaction.PayedAmount = transakResponse.Response.AmountPaid;
                            var rate = rates.FirstOrDefault(r => r.Ticker == transaction.Currency);
                            transaction.PayedInUSD = transakResponse.Response.FiatAmountInUsd;
                            transaction.FeeInUSD = transakResponse.Response.TotalFeeInFiat * rate.Rate;
                            transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, transakResponse.Response.TotalFeeInFiat, transakResponse.Response.AmountPaid);
                            transaction.MosaicoFeeInUSD = transaction.MosaicoFee * rate.Rate;
                            
                            if (transactions.Any(t =>
                                t.Id != transaction.Id && t.CorrelationId == transaction.CorrelationId &&
                                t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed))
                            {
                                _walletDbContext.Transactions.Remove(transaction);
                                await _walletDbContext.SaveChangesAsync();
                                continue;
                            }
                            
                            if (transakResponse.Response.WalletAddress.ToLower() == transaction.IntermediateAddress.ToLower())
                            {
                                var currentStage = await _projectManagementClient.GetStageAsync(transaction.StageId.Value);
                                transaction.TokenAmount = transaction.PayedInUSD / currentStage.TokenPrice;
                                await _crowdsalePurchaseService.AddTokenToUserWalletAsync(transaction.TokenId.Value, transaction.To, transaction.Network);
                                var transactionHash = await _crowdsalePurchaseService.TransferTargetTokensToUserAsync(transaction);
                                transaction.TransactionHash = transactionHash;
                                await _walletDbContext.SaveChangesAsync();
                                await _crowdsaleDispatcher.PurchaseSuccessful(transaction.UserId, transaction.TransactionHash);
                                await PublishSuccessEventAsync(transaction);
                                await PublishStatisticEventsAsync(transaction);
                            }
                            else
                            {
                                transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Failed));
                                transaction.FailureReason = "Invalid receiver wallet";
                                await _walletDbContext.SaveChangesAsync();
                                await _crowdsaleDispatcher.PurchaseFailed(transaction.UserId, "Payment failed");
                                await PublishFailEventAsync(transaction);
                            }
                        }
                        else if (transakResponse.Response.Status.ToUpper()  == Payments.Transak.Constants.TransactionStatuses.CANCELLED ||
                            transakResponse.Response.Status.ToUpper()  == Payments.Transak.Constants.TransactionStatuses.EXPIRED)
                        {
                            transaction.SetStatus(statuses.FirstOrDefault(s => s.Key == Domain.Wallet.Constants.TransactionStatuses.Canceled));
                            await _walletDbContext.SaveChangesAsync();
                            await _crowdsaleDispatcher.PurchaseFailed(transaction.UserId, "Payment was cancelled or expired");
                            await PublishFailEventAsync(transaction);
                        }
                        else if (transakResponse.Response.Status.ToUpper()  == Payments.Transak.Constants.TransactionStatuses.FAILED)
                        {
                            transaction.FinishedAt = _provider.Now();
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