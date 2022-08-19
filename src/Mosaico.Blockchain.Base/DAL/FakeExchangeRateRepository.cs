using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.Exceptions;

namespace Mosaico.Blockchain.Base.DAL
{
    public class FakeExchangeRateRepository : IExchangeRateRepository
    {
        private readonly Dictionary<string, decimal> _exchangeRates = new Dictionary<string, decimal>
        {
            {"EUR", 1.11m},
            {"PLN", 0.28m},
            {"USD", 1m},
            {"GBP", 1.3m},
            {"ETH", 2928m},
            {"BTC", 43517m},
            {"USDC", 1m},
            {"USDT", 1m},
            {"MATIC", 1.6m},
            {"BUSD", 1m}
        };
        
        public Task<decimal> GetExchangeRateAsync(string currencyTicker, string baseCurrencyTicker = "USD")
        {
            if (!_exchangeRates.ContainsKey(currencyTicker) || !_exchangeRates.ContainsKey(baseCurrencyTicker))
            {
                throw new UnsupportedChainException(currencyTicker);
            }

            return Task.FromResult(_exchangeRates[currencyTicker]);
        }

        public Task<Dictionary<string, decimal>> GetUsdExchangeAssetsAsync()
        {
            return Task.FromResult(_exchangeRates);
        }
    }
}