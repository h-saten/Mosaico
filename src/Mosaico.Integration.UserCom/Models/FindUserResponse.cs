using Newtonsoft.Json;

namespace Mosaico.Integration.UserCom.Models
{
    public class FindUserResponse
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}