using Newtonsoft.Json;

namespace Mosaico.SDK.Base.Models
{
    public class SuccessResponse<T>
    {
        public SuccessResponse(T data)
        {
            Data = data;
            Ok = true;
        }

        public SuccessResponse()
        {
            
        }
        
        [JsonProperty("data")]
        public T Data { get; set; }
        
        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }
}