using System.Threading.Tasks;
using Mosaico.Payments.RampNetwork.Abstractions;
using Mosaico.Payments.RampNetwork.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Mosaico.Payments.RampNetwork
{
    public class RampClient : IRampClient
    {
        public async Task<RampPurchase> GetPurchaseAsync(string id, string secret)
        {
            var restApiClient = new RestClient("https://api-instant.ramp.network");
            var response = await restApiClient.ExecuteGetAsync(new RestRequest($"/api/host-api/purchase/{id}?secret={secret}"));
            return response.IsSuccessful ? JsonConvert.DeserializeObject<RampPurchase>(response.Content) : null;
        }
    }
}