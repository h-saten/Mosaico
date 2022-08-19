using Newtonsoft.Json;

namespace Mosaico.Integration.UserCom.Models.Request
{
    public class CreateConversation
    {
        [JsonProperty(PropertyName = "assigned_to")]
        public int AssignedTo { get; set; }
        
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
        
        [JsonProperty(PropertyName = "source_context")]
        public string SourceContext { get; set; }
        
        [System.Text.Json.Serialization.JsonIgnore]
        public int UserId { get; set; }
    }
}