using System.Threading.Tasks;
using Mosaico.KYC.Passbase.Abstractions;
using Mosaico.KYC.Passbase.Configurations;
using Mosaico.KYC.Passbase.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Mosaico.KYC.Passbase
{
    public class PassbaseClient : IPassbaseClient
    {
        private readonly PassbaseConfig _config;

        public PassbaseClient(PassbaseConfig config)
        {
            _config = config;
        }

        public async Task<PassbaseIdentity> GetIdentityAsync(string id)
        {
            var restApiClient = new RestClient(_config.Url);
            var key = _config.ApiSecret;
            var request = new RestRequest($"/verification/v1/identities/{id}");
            request.AddHeader("X-API-KEY", key);
            var response = await restApiClient.ExecuteGetAsync(request);
            return response.IsSuccessful ? JsonConvert.DeserializeObject<PassbaseIdentity>(response.Content) : null;
        }
    }
}