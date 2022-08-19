using Newtonsoft.Json;

namespace Mosaico.Payments.Transak.Configurations
{
    public class TransakConfiguration
    {
        public const string SectionName = "Transak";
        
        [JsonProperty("ApiSecret")]
        public string ApiSecret { get; set; }
    }
}