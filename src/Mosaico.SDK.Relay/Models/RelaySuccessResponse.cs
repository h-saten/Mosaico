using Newtonsoft.Json;

namespace Mosaico.SDK.Relay.Models
{
    public class RelaySuccessResponse<T>
    {
        public RelaySuccessResponse()
        {
            Ok = true;
        }
        
        [JsonProperty("data")]
        public T Data { get; set; }
        
        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }
}