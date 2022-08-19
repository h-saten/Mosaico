using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mosaico.Integration.Sms.SmsLabs.Models
{
    public class SmsLabsResponse<TData, TError> where TData : class
    {
        [JsonProperty("meta")]
        public MetaResponse Meta { get; set; }
        
        [JsonProperty("data")]
        public virtual IReadOnlyList<TData> Data { get; set; }
        
        [JsonProperty("errors")]
        public virtual IReadOnlyList<TError> Errors { get; set; }
    }
}