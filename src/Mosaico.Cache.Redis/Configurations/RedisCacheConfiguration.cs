using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mosaico.Cache.Redis.Configurations
{
    public class RedisCacheConfiguration
    {
        public const string SectionName = "Cache";
        
        [JsonProperty("RedisConnectionString")]
        public string RedisConnectionString { get; set; }
        
        [JsonProperty("RedisDatabase")]
        public int RedisDatabase { get; set; }
        
        [JsonProperty("Mappings")]
        public Dictionary<string, string> Mappings { get; set; }
        
        [JsonProperty("IsEnabled")]
        public bool IsEnabled { get; set; }
    }
}