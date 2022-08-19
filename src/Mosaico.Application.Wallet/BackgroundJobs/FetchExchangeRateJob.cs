using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.FetchExchangeRatesJob, IsRecurring = true)]
    public class FetchExchangeRateJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly ILogger _logger;

        public FetchExchangeRateJob(IWalletDbContext walletDbContext, IExchangeRateRepository exchangeRateRepository, IProjectManagementClient projectManagementClient, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _exchangeRateRepository = exchangeRateRepository;
            _projectManagementClient = projectManagementClient;
            _logger = logger;
        }
        
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            _logger?.Verbose($"Starting to fetch currency exchange rates");
            var exchangeRates = await _exchangeRateRepository.GetUsdExchangeAssetsAsync();
            _logger?.Verbose($"Exchange Rate Repository returned {exchangeRates.Count} results");
            var cryptoRates = exchangeRates.Where(d => Constants.CryptoCurrencies.All.Contains(d.Key) && d.Value > 0);
            var fiatRates = exchangeRates.Where(d => Constants.FIATCurrencies.All.Contains(d.Key) && d.Value > 0);
            foreach (var rate in cryptoRates)
            {
                _walletDbContext.ExchangeRates.Add(new ExchangeRate
                {
                    Rate = rate.Value,
                    Source = Mosaico.Integration.Blockchain.CoinAPI.Constants.ExchangeRateSource,
                    Ticker = rate.Key,
                    BaseCurrency = Constants.FIATCurrencies.USD,
                    IsCrypto = true
                });
            }

            foreach (var rate in fiatRates)
            {
                _walletDbContext.ExchangeRates.Add(new ExchangeRate
                {
                    Rate = rate.Key == "USD" && rate.Value == 0 ? 1 : rate.Value,
                    Source = Integration.Blockchain.CoinAPI.Constants.ExchangeRateSource,
                    Ticker = rate.Key,
                    BaseCurrency = Constants.FIATCurrencies.USD,
                    IsCrypto = false
                });
            }

            var tokens = await _walletDbContext.Tokens.ToListAsync();
            foreach (var token in tokens)
            {
                var projects = await _projectManagementClient.GetProjectsByTokenAsync(token.Id);
                var latestTokenPrice = projects.SelectMany(p => p.Stages).OrderByDescending(s => s.EndDate).FirstOrDefault()?.TokenPrice;
                if (latestTokenPrice > 0)
                {
                    var tokenExchangeRate = await _walletDbContext.ExchangeRates.FirstOrDefaultAsync(we => we.Ticker == token.Symbol);
                    if (tokenExchangeRate == null)
                    {
                        tokenExchangeRate = new ExchangeRate
                        {
                            Rate = latestTokenPrice.Value,
                            Source = "PROJECTS",
                            Ticker = token.Symbol,
                            BaseCurrency = Constants.FIATCurrencies.USD,
                            IsCrypto = true
                        };
                        _walletDbContext.ExchangeRates.Add(tokenExchangeRate);
                    }
                    else
                    {
                        tokenExchangeRate.Rate = latestTokenPrice.Value;
                    }
                }
            }

            await _walletDbContext.SaveChangesAsync();
            _logger?.Information($"Successfully added {cryptoRates.Count() + fiatRates.Count()} rates to database");
        }
    }
}