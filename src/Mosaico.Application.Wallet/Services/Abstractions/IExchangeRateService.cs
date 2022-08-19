using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRate>> GetExchangeRatesAsync(string baseCurrency = "USD", List<string> whitelisted = null);
        Task<ExchangeRate> GetExchangeRateAsync(string currency, string baseCurrency = "USD");
    }
}