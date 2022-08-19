using Newtonsoft.Json;

namespace Mosaico.Integration.Sms.SmsLabs.Configurations
{
    public class SmsLabsConfig
    {
        public const string SectionName = "SmsLabs";

        [JsonProperty("Url")]
        public string Url { get; set; }
        
        [JsonProperty("SenderId")]
        public string SenderId { get; set; }
        
        [JsonProperty("AppKey")]
        public string AppKey { get; set; }
        
        [JsonProperty("SecretKey")]
        public string SecretKey { get; set; }
    }
}