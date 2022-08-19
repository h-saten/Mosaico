using Newtonsoft.Json;

namespace Mosaico.Integration.Email.EmailLabs.Configurations
{
    public class EmailLabsConfig
    {
        public const string SectionName = "EmailLabs";
        
        [JsonProperty("Url")]
        public string Url { get; set; }
        
        [JsonProperty("SmtpAccount")]
        public string SmtpAccount { get; set; }

        [JsonProperty("FromEmail")]
        public string FromEmail { get; set; }
        
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }
        
        [JsonProperty("AppKey")]
        public string AppKey { get; set; }
        
        [JsonProperty("SecretKey")]
        public string SecretKey { get; set; }
    }
}