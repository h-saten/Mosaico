using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mosaico.BackgroundJobs.Hangfire.Configurations
{
    public class HangfireConfig
    {
        public const string SectionName = "Hangfire";

        [JsonProperty("IsEnabled")] 
        public bool IsEnabled { get; set; }

        [JsonProperty("ConnectionString")]
        public string ConnectionString { get; set; }
        
        [JsonProperty("IsDashboardEnabled")]
        public bool IsDashboardEnabled { get; set; }
        
        [JsonProperty("DatabaseSchema")]
        public string DatabaseSchema { get; set; }
        
        [JsonProperty("JobSchedule")]
        public Dictionary<string, string> JobSchedule { get; set; } = new Dictionary<string, string>();

        [JsonProperty("AccountDeletionFrequency")]
        public int AccountDeletionFrequency { get; set; }
        
        [JsonProperty("DashboardPrefix")]
        public string DashboardPrefix { get; set; }
        
        [JsonProperty("DashboardUrl")]
        public string DashboardUrl { get; set; }
        
        [JsonProperty("AccessPassword")]
        public string AccessPassword { get; set; }
    }
}