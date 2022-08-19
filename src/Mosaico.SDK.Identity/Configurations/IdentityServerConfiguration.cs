using Newtonsoft.Json;

namespace Mosaico.SDK.Identity.Configurations
{
    public class IdentityServerConfiguration
    {
        public const string SectionName = "IdentityServer";
        
        [JsonProperty("Authority")]
        public string Authority { get; set; }
        
        [JsonProperty("Secret")]
        public string Secret { get; set; }
        
        [JsonProperty("ClientId")]
        public string ClientId { get; set; }
        
        [JsonProperty("Url")]
        public string Url { get; set; }
        
        [JsonProperty("Api")]
        public IdentityApiConfiguration Api { get; set; }
    }
}