using Newtonsoft.Json;

namespace Mosaico.Integration.Sms.SmsLabs.Models.SendSms
{
    public class SendSmsErrorDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("details")]
        public string Details { get; set; }
    }
}