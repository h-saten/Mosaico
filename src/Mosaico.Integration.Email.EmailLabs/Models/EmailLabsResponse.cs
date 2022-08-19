using Newtonsoft.Json;

namespace Mosaico.Integration.Email.EmailLabs.Models
{
    public class EmailLabsResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("data")]
        public object Data { get; set; }
        
        [JsonProperty("req_id")]
        public string RequestId { get; set; }
    }
}