using Newtonsoft.Json;

namespace KangaExchange.SDK.Configurations
{
    public class KangaApiConfiguration
    {
        public const string SectionName = "KangaApi";
        
        [JsonProperty("AppId")]
        public string AppId { get; set; }
        
        [JsonProperty("AppSecret")]
        public string AppSecret { get; set; }
        
        [JsonProperty("BaseUrl")]
        public string BaseUrl { get; set; }
        
        [JsonProperty("V1Key")]
        public string V1Key { get; set; }
        
        [JsonProperty("PaymentBaseUrl")]
        public string PaymentBaseUrl { get; set; }
        
        [JsonProperty("TransactionCallback")]
        public string TransactionCallback { get; set; }
    }
}