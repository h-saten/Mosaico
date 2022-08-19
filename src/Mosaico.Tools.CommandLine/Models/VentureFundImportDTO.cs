using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mosaico.Tools.CommandLine.Models
{
    public class VentureTokenImportDTO
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Logo { get; set; }
        public bool IsStakingEnabled { get; set; }
        public decimal LatestPrice { get; set; }
    }
    
    public class VentureFundImportDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }
        
        [JsonProperty("lastUpdatedAt")]
        public DateTimeOffset LastUpdatedAt { get; set; }
        
        [JsonProperty("tokens")]
        public List<VentureTokenImportDTO> Tokens { get; set; }
    }
}