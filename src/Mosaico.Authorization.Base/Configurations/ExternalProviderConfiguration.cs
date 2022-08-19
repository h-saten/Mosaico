using Newtonsoft.Json;

namespace Mosaico.Authorization.Base.Configurations
{
    public class ExternalProviderConfiguration
    {
        [JsonProperty("ClientId")]
        public string ClientId { get; set; }
        
        [JsonProperty("ClientSecret")]
        public string ClientSecret { get; set; }
        
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
    }
}