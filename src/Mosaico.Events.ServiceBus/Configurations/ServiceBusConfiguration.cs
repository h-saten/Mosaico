using Newtonsoft.Json;

namespace Mosaico.Events.ServiceBus.Configurations
{
    public class ServiceBusConfiguration
    {
        public const string SectionName = "ServiceBus";
        
        [JsonProperty("ConnectionString")]
        public string ConnectionString { get; set; }
        
        [JsonProperty("Prefix")]
        public string Prefix { get; set; }
    }
}