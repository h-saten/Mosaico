using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.KYC.Passbase.Models
{
    public enum PassbaseVerificationEventType
    {
        VERIFICATION_COMPLETED = 0,
        VERIFICATION_REVIEWED = 1
    }
    
    public class PassbaseVerificationPayload
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PassbaseVerificationEventType Event { get; set; }
        
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("created")]
        public long Created { get; set; }
        
        [JsonProperty("updated")]
        public long Updated { get; set; }
        
        [JsonProperty("processed")]
        public long Processed { get; set; }
    }
}