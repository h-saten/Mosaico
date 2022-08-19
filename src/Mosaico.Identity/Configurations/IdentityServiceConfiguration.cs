using System.Collections.Generic;
using Mosaico.Base.Configurations;
using Newtonsoft.Json;

namespace Mosaico.Identity.Configurations
{
    public class IdentityServiceConfiguration
    {
        public const string SectionName = "Service";
        [JsonProperty("AllowedOrigins")]
        public List<string> AllowedOrigins { get; set; }
        
        [JsonProperty("RunMigrations")]
        public bool RunMigrations { get; set; }
        
        [JsonProperty("EmailProvider")]
        public string EmailProvider { get; set; }
        
        [JsonProperty("SmsProvider")]
        public string SmsProvider { get; set; }
        
        [JsonProperty("ApiClientSecret")]
        public string ApiClientSecret { get; set; }
        
        [JsonProperty("IdentityClientSecret")]
        public string IdentityClientSecret { get; set; }
        
        [JsonProperty("RedirectUris")]
        public List<string> RedirectUris { get; set; } = new List<string>();

        [JsonProperty("PostLogoutRedirectUris")]
        public List<string> PostLogoutRedirectUris { get; set; } = new List<string>();

        [JsonProperty("RecreateResources")]
        public bool RecreateResources { get; set; }
        
        [JsonProperty("KeyVaultCertificateName")]
        public string KeyVaultCertificateName { get; set; }
        
        [JsonProperty(CertificateConfiguration.SectionName)]
        public CertificateConfiguration Certificate { get; set; }
        
        [JsonProperty("IssuerUri")]
        public string IssuerUri { get; set; }
        
        [JsonProperty("AfterLoginRedirectUrl")]
        public string AfterLoginRedirectUrl { get; set; }
        
        [JsonProperty("BaseUri")]
        public string BaseUri { get; set; }
        
        [JsonProperty("SpaURL")]
        public string SpaURL { get; set; }
        
        [JsonProperty("RecaptchaSiteKey")]
        public string RecaptchaSiteKey { get; set; }
        
        [JsonProperty("GatewayUrl")]
        public string GatewayUrl { get; set; }
        
        [JsonProperty("SwaggerEnabled")]
        public bool SwaggerEnabled { get; set; }
    }
}