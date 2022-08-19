using System.Collections.Generic;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Models;
using Newtonsoft.Json;
using RestSharp;

namespace KangaExchange.SDK
{
    public class KangaMarketApiClient : IKangaMarketApiClient
    {
        private readonly KangaConfiguration _kangaConfiguration;

        public KangaMarketApiClient(KangaConfiguration kangaConfiguration)
        {
            _kangaConfiguration = kangaConfiguration;
        }

        public async Task<List<KangaMarket>> GetMarketsAsync()
        {
            var restApiClient = new RestClient(_kangaConfiguration.Api.BaseUrl);
            var response = await restApiClient.ExecutePostAsync(new RestRequest($"/{Constants.KangaAPIRoutes.Markets}"));
            if (!response.IsSuccessful) return null;
            var kangaResponse = JsonConvert.DeserializeObject<KangaResponse>(response.Content);
            return kangaResponse?.Items;
        }
    }
}