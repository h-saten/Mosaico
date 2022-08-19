using Newtonsoft.Json;

namespace Mosaico.Events.RabbitMq.Configurations
{
    public class RabbitMQConfiguration
    {
        public const string SectionName = "RabbitMQ";
        
        [JsonProperty("Host")]
        public string Host { get; set; }
        
        [JsonProperty("Prefix")]
        public string Prefix { get; set; }
    }
}