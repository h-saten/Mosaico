using Newtonsoft.Json;

namespace Mosaico.Tools.CommandLine.Models
{
    public class PaymentCurrencyImportDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("network")]
        public string Network { get; set; }
        
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }
        
        [JsonProperty("decimals")]
        public int Decimals { get; set; }
        
        [JsonProperty("isNative")]
        public bool IsNative { get; set; }
    }
}