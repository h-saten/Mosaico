using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mosaico.Integration.Sms.SmsLabs.Models.SendSms
{
    public class SendSmsRequest
    {
        [JsonProperty("phone_number")]
        public List<string> PhoneNumber { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("sender_id")]
        public string SenderId { get; set; }

        [JsonProperty("no_polish_signs")]
        public int NoPolishSigns { get; set; } = 0;
    }
}