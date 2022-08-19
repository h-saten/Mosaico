using Newtonsoft.Json;

namespace KangaExchange.SDK.Models
{
    public class ReportBodyDTO
    {
        [JsonProperty("appId")]
        public string AppId { get; set; }
        
        [JsonProperty("nonce")]
        public long Nonce { get; set; }
        
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}