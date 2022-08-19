using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Tools.CommandLine.Models
{
    public class TokenImportModel
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
        
        public bool DisplayAlways { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ERCType Type { get; set; }
    }
}