using System.Threading.Tasks;
using Mosaico.Payments.Transak.Abstractions;
using Mosaico.Payments.Transak.Configurations;
using Mosaico.Payments.Transak.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Mosaico.Payments.Transak
{
    public class TransakClient : ITransakClient
    {
        private readonly TransakConfiguration _transakConfiguration;

        public TransakClient(TransakConfiguration transakConfiguration)
        {
            _transakConfiguration = transakConfiguration;
        }

        public async Task<TransakResponse<OrderDetails>> GetOrderDetailsAsync(string orderId)
        {
            var restApiClient = new RestClient("https://api.transak.com/api/v2");
            var request = new RestRequest($"/partners/order/{orderId}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddQueryParameter("partnerAPISecret", _transakConfiguration.ApiSecret);
            var response = await restApiClient.ExecuteGetAsync(request);
            return response.IsSuccessful ? JsonConvert.DeserializeObject<TransakResponse<OrderDetails>>(response.Content) : null;
        }
    }
}