using Newtonsoft.Json;

namespace Mosaico.Integration.UserCom.Models.Request
{
    public class CreateUser
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }
    }
}