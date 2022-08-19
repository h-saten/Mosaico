using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.ExpireTransactionsJob, IsRecurring = true, Cron = "0 */1 * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ExpirePendingTransactionsJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ILogger _logger;
        
        public ExpirePendingTransactionsJob(IWalletDbContext walletDbContext, IDateTimeProvider timeProvider, ILogger logger, IEventPublisher eventPublisher, IEventFactory eventFactory)
        {
            _walletDbContext = walletDbContext;
            _timeProvider = timeProvider;
            _logger = logger;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var now = _timeProvider.Now();
            var transactions = await _walletDbContext.Transactions.Where(t =>
                t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending
                && t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase).ToListAsync();
            _logger?.Information($"Found {transactions.Count} to expire. Starting operation...");
            var expiredStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Expired);
            var expiredTransactionIds = new List<Guid>();
            
            foreach (var transaction in transactions)
            {
                if (now > transaction.CreatedAt.AddDays(7))
                {
                    transaction.SetStatus(expiredStatus);
                    expiredTransactionIds.Add(transaction.Id);
                    _walletDbContext.Transactions.Update(transaction);
                }
            }
            await _walletDbContext.SaveChangesAsync();
            if (expiredTransactionIds.Any())
            {
                await PublishEvents(expiredTransactionIds);
            }
        }

        private async Task PublishEvents(List<Guid> expiredTransactions)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, new TransactionsExpired(expiredTransactions));
            await _eventPublisher.PublishAsync(e);
        }
    }
}