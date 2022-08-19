using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.ExpireTokenLocksBackgroundJob, IsRecurring = true, Cron = "*/1 * * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ExpireTokenLocksBackgroundJob : HangfireBackgroundJobBase
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenLockService _lockService;
        private readonly IDateTimeProvider _timeProvider;

        public ExpireTokenLocksBackgroundJob(ILogger logger, IWalletDbContext walletDbContext, ITokenLockService lockService, IDateTimeProvider timeProvider)
        {
            _logger = logger;
            _walletDbContext = walletDbContext;
            _lockService = lockService;
            _timeProvider = timeProvider;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var now = _timeProvider.Now();
            // Time Base Token Locks
            var tokenLocks = await _walletDbContext.TokenLocks
                .Where(t => !t.Expired && t.ExpiresAt.HasValue && t.ExpiresAt > now)
                .ToListAsync();
            foreach (var tokenLock in tokenLocks)
            {
                await _lockService.SetExpiredAsync(tokenLock);
            }
        }
    }
}