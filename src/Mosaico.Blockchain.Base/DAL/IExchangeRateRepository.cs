using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Blockchain.Base.DAL
{
    public interface IExchangeRateRepository
    {
        Task<decimal> GetExchangeRateAsync(string currencyTicker, string baseCurrencyTicker = "USD");
        Task<Dictionary<string, decimal>> GetUsdExchangeAssetsAsync();
    }
}