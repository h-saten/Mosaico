using Newtonsoft.Json;

namespace KangaExchange.SDK.Configurations
{
    public class KangaConfiguration
    {
        public const string SectionName = "Kanga";
        
        [JsonProperty("BaseUrl")]
        public string BaseUrl { get; set; }
        
        [JsonProperty(KangaApiConfiguration.SectionName)]
        public KangaApiConfiguration Api { get; set; }
        
        [JsonProperty("AfterLoginRedirectUrl")]
        public string AfterLoginRedirectUrl { get; set; }
        
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
        
        [JsonProperty("OrderExpireAfterDays")]
        public int OrderExpireAfterDays { get; set; }

        [JsonProperty("AfterPurchaseRedirectUrl")]
        public string AfterPurchaseRedirectUrl { get; set; }
    }
}