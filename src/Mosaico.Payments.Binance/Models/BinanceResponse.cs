using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceResponse<T>
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}