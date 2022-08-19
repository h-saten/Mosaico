using Newtonsoft.Json;

namespace Mosaico.Blockchain.Base.DAL.Models
{
    public class ERC20Balance
    {
        [JsonProperty("token_address")]
        public string TokenAddress { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("logo")]
        public string Logo { get; set; }
        
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
        
        [JsonProperty("decimals")]
        public string Decimals { get; set; }
        
        [JsonProperty("string")]
        public string Balance { get; set; }
    }
}