using System.Collections.Generic;
using Mosaico.Base.Configurations;
using Newtonsoft.Json;

namespace Mosaico.Core.Service.Configurations
{
    public class ServiceConfiguration
    {
        public const string SectionName = "Service";
        
        public string AuthType { get; set; }
        public string EmailProvider { get; set; }
        public string SmsProvider { get; set; }
        public List<string> AllowedOrigins { get; set; }
        public bool RunMigrations { get; set; }
        
        [JsonProperty(CertificateConfiguration.SectionName)]
        public CertificateConfiguration Certificate { get; set; }
        public string BaseUri { get; set; }
        public string AnalyticsProvider { get; set; }
        
        [JsonProperty("SwaggerEnabled")]
        public bool SwaggerEnabled { get; set; }
    }
}