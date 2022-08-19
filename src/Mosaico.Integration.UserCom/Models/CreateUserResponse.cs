using Newtonsoft.Json;

namespace Mosaico.Integration.UserCom.Models
{
    public class CreateUserResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}