using System.Threading.Tasks;
using Hangfire;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.FetchRahimCoinPriceJob, IsRecurring = true, Cron = "0 */1 * * *" )]
    public class FetchRahimCoinPriceJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;

        public FetchRahimCoinPriceJob(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            _walletDbContext.ExchangeRates.Add(new ExchangeRate
            {
                Rate = 1,
                Source = "STATIC",
                Ticker = "RC",
                BaseCurrency = Constants.FIATCurrencies.USD,
                IsCrypto = true
            });
            await _walletDbContext.SaveChangesAsync();
        }
    }
}