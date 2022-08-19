using Newtonsoft.Json;

namespace Mosaico.Core.EntityFramework.Configurations
{
    public class SqlServerConfiguration
    {
        public const string SectionName = "SqlServer";

        [JsonProperty("ConnectionString")] 
        public string ConnectionString { get; set; }
    }
}