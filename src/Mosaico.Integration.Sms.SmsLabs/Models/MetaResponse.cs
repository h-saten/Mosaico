using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mosaico.Integration.Sms.SmsLabs.Models
{
    public class MetaResponse
    {
        [JsonProperty("req_id")]
        public string RequestId { get; set; }
        
        [JsonProperty("number_of_elements")]
        public int NumberOfElements { get; set; }
        
        [JsonProperty("number_of_errors")]
        public int NumberOfErrors { get; set; }
        
        [JsonProperty("errors")]
        public IReadOnlyList<object> Errors { get; set; }
    }
}