using Newtonsoft.Json;

namespace Mosaico.Integration.Blockchain.CoinAPI.Configurations
{
    public class CoinApiConfiguration
    {
        public const string SectionName = "CoinAPI";
        
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; }
        
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
    }
}