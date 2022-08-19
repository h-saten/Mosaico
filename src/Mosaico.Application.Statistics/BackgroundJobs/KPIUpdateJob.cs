using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Mosaico.Application.Statistics.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Statistics.Abstractions;

namespace Mosaico.Application.Statistics.BackgroundJobs
{
    [BackgroundJob(Statistics.Constants.Jobs.KPIUpdateJob, IsRecurring = true, Cron = "0 */1 * * *", ExecutedOnStartup = true)]
    public class KPIUpdateJob : HangfireBackgroundJobBase
    {
        private readonly IKPIService _kpiService;

        private readonly Dictionary<string, decimal> _defaultKpi = new()
        {
            {"TOTAL_GATHERED_CAPITAL", 4585058},
            {"TOTAL_PAID_IN_REWARDS", 5332.74m},
            {"ANNUAL_FUND_GROWTH", 20m}
        };

        public KPIUpdateJob(IStatisticsDbContext statisticsContext, IKPIService kpiService)
        {
            _kpiService = kpiService;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            if (_defaultKpi.Any())
            {
                foreach (var kpi in _defaultKpi)
                {
                    await _kpiService.CreateOrUpdateKPIAsync(kpi.Key, kpi.Value.ToString(CultureInfo.InvariantCulture));
                }
            }
        }
    }
}