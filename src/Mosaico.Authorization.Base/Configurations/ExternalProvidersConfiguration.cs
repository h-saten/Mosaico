using Newtonsoft.Json;

namespace Mosaico.Authorization.Base.Configurations
{
    public class ExternalProvidersConfiguration
    {
        public const string SectionName = "ExternalProviders";
        
        [JsonProperty("ErrorRedirectUrl")]
        public string ErrorRedirectUrl { get; set; }
        
        [JsonProperty("Facebook")]
        public ExternalProviderConfiguration Facebook { get; set; }
        
        [JsonProperty("Google")]
        public ExternalProviderConfiguration Google { get; set; }
    }
}