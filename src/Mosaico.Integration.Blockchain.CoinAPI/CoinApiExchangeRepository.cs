using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinAPI.REST.V1;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Integration.Blockchain.CoinAPI.Configurations;

namespace Mosaico.Integration.Blockchain.CoinAPI
{
    public class CoinApiExchangeRepository : IExchangeRateRepository
    {
        private readonly CoinApiConfiguration _configuration;

        public CoinApiExchangeRepository(CoinApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<decimal> GetExchangeRateAsync(string currencyTicker, string baseCurrencyTicker = "USD")
        {
            var client = new CoinApiRestClient(_configuration.ApiKey, false);
            var rateResponse = await client.Exchange_rates_get_specific_rateAsync(currencyTicker, baseCurrencyTicker);
            return rateResponse.rate;
        }

        public async Task<Dictionary<string, decimal>> GetUsdExchangeAssetsAsync()
        {
            var client = new CoinApiRestClient(_configuration.ApiKey, false);
            var response = await client.Metadata_list_assetsAsync();
            return response.ToDictionary(a => a.asset_id, a => a.price_usd ?? 0);
        }
    }
}