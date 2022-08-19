using Newtonsoft.Json;

namespace Mosaico.Integration.Sms.SmsLabs.Models.SendSms
{
    public class SendSmsDataDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("number")]
        public string PhoneNumber { get; set; }
    }
}