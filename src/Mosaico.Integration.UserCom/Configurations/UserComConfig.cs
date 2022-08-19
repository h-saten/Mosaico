using Newtonsoft.Json;

namespace Mosaico.Integration.UserCom.Configurations
{
    public class UserComConfig
    {
        public const string SectionName = "UserCom";
        
        [JsonProperty("Url")]
        public string Url { get; set; }
        
        [JsonProperty("AuthorizationToken")]
        public string AuthorizationToken { get; set; }
    }
}