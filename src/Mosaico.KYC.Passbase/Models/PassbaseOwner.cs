using Newtonsoft.Json;

namespace Mosaico.KYC.Passbase.Models
{
    public class PassbaseOwner
    {
        public string Email { get; set; }
        
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}