using Newtonsoft.Json;

namespace Mosaico.EventSourcing.Redis.Configurations
{
    public class RedisEventSourcingConfiguration
    {
        public const string SectionName = "EventSourcing";
        
        [JsonProperty("RedisConnectionString")]
        public string RedisConnectionString { get; set; }
        
        [JsonProperty("RedisDatabase")]
        public int RedisDatabase { get; set; }
        
        [JsonProperty("StreamName")]
        public string StreamName{ get; set; }
    }
}