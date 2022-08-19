using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IWalletDbContext _walletDbContext;

        public ExchangeRateService(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public Task<ExchangeRate> GetExchangeRateAsync(string currency, string baseCurrency = "USD")
        {
            return _walletDbContext.ExchangeRates.OrderByDescending(e => e.CreatedAt)
                .FirstOrDefaultAsync(t => t.Ticker == currency && t.BaseCurrency == baseCurrency);
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesAsync(string baseCurrency = "USD", List<string> whitelisted = null)
        {
            whitelisted ??= new List<string> {"EUR", "PLN", "USD", "GBP"};

            var rates = new List<ExchangeRate>();
            var baseQuery = _walletDbContext.ExchangeRates.AsNoTracking()
                .Where(c => c.BaseCurrency == baseCurrency)
                .OrderByDescending(er => er.CreatedAt);
            foreach (var currencySymbol in whitelisted)
            {
                var rate = await baseQuery.FirstOrDefaultAsync(t => t.Ticker == currencySymbol);
                if (rate != null)
                {
                    rates.Add(rate);
                }
            }
            return rates;
        }
    }
}