using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Configurations
{
    public class BinanceConfiguration
    {
        public const string SectionName = "Binance";
        
        [JsonProperty("ApiSecret")]
        public string ApiSecret { get; set; }
        
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; }
        
        [JsonProperty("Url")]
        public string Url { get; set; }
        
        [JsonProperty("RedirectUrl")]
        public string RedirectUrl { get; set; }
    }
}