using Newtonsoft.Json;

namespace Mosaico.Authorization.Base.Configurations
{
    public class AuthenticatorConfiguration
    {
        public const string SectionName = "Authenticator";

        [JsonProperty("OtpDomain")]
        public string OtpDomain { get; set; }
        
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
        
        [JsonProperty("AuthenticatorUriFormat")]
        public string AuthenticatorUriFormat { get; set; }
    }
}