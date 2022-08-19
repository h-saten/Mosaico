using Newtonsoft.Json;

namespace Mosaico.Base.Configurations
{
    public class CertificateConfiguration
    {
        public const string SectionName = "Certificate";
        
        [JsonProperty("Password")]
        public string Password { get; set; }
        
        [JsonProperty("FileName")]
        public string FileName { get; set; }
    }
}