using Newtonsoft.Json;

namespace Mosaico.SDK.Relay.Models
{
    public class RelayErrorResponse
    {
        public RelayErrorResponse()
        {
            Ok = false;
        }
        
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        
        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}