using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Models;
using Microsoft.EntityFrameworkCore;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.KangaMarketFetchJob, IsRecurring = true, Cron = "0 */1 * * *" )]
    public class FetchKangaMarketsJob : HangfireBackgroundJobBase
    {
        private readonly IKangaMarketApiClient _kangaMarket;
        private readonly IVentureFundDbContext _fundDbContext;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;

        public FetchKangaMarketsJob(IKangaMarketApiClient kangaMarket, IWalletDbContext walletDbContext, IVentureFundDbContext fundDbContext, ILogger logger = null)
        {
            _kangaMarket = kangaMarket;
            _walletDbContext = walletDbContext;
            _fundDbContext = fundDbContext;
            _logger = logger;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            try
            {
                _logger.Information($"Attempting to fetch market data from Kanga");
                var kangaMarkets = await _kangaMarket.GetMarketsAsync();
                _logger.Information($"Gathered {kangaMarkets.Count} exchange rates on Kanga Exchange");
                var plnExchangeRate = await _walletDbContext.ExchangeRates.OrderByDescending(er => er.CreatedAt).Where(
                        t =>
                            t.Ticker == Constants.FIATCurrencies.PLN && t.BaseCurrency == Constants.FIATCurrencies.USD)
                    .FirstOrDefaultAsync();

                var markets = kangaMarkets.Where(t => KangaExchange.SDK.Constants.WhiteListedMarkets.Contains(t.BuyingCurrency) && 
                    (t.PayingCurrency == Constants.CryptoCurrencies.oPLN || t.PayingCurrency == Constants.CryptoCurrencies.USDT));
                foreach (var market in markets)
                {
                    var baseCurrency = Constants.FIATCurrencies.USD;
                    var currentPrice = GetMarketPriceInUSD(market, plnExchangeRate);
                    if (currentPrice > 0)
                    {
                        if (KangaExchange.SDK.Constants.KangaMarketMapping.ContainsKey(market.BuyingCurrency))
                        {
                            market.BuyingCurrency = KangaExchange.SDK.Constants.KangaMarketMapping[market.BuyingCurrency];
                        }

                        _walletDbContext.ExchangeRates.Add(new ExchangeRate
                        {
                            Rate = currentPrice,
                            Source = KangaExchange.SDK.Constants.MarketProviderName,
                            Ticker = market.BuyingCurrency,
                            BaseCurrency = baseCurrency,
                            IsCrypto = true
                        });
                    }
                }

                await _walletDbContext.SaveChangesAsync();
                
                await UpdateVentureFundsAsync(markets, plnExchangeRate);
            }
            catch(Exception ex) 
            {
                _logger?.Error("Error during Kanga Market Fetch", ex);
                throw;
            }
        }

        private decimal GetMarketPriceInUSD(KangaMarket market, ExchangeRate plnExchangeRate)
        {
            var currentPrice = decimal.Parse(market.LastPrice);
            if (currentPrice > 0)
            {
                if (market.PayingCurrency == Constants.CryptoCurrencies.oPLN)
                {
                    currentPrice *= plnExchangeRate.Rate;
                }
                return currentPrice;
            }

            return 0;
        }
        
        private async Task UpdateVentureFundsAsync(IEnumerable<KangaMarket> markets, ExchangeRate plnExchangeRate) 
        {
            foreach (var market in markets)
            {
                var ventureTokens = await _fundDbContext.VentureFundTokens.Where(t => t.Symbol == market.BuyingCurrency).ToListAsync();
                foreach (var ventureFundToken in ventureTokens)
                {
                    ventureFundToken.LatestPrice = GetMarketPriceInUSD(market, plnExchangeRate);
                }
            }
            await _fundDbContext.SaveChangesAsync();
        }
    }
}