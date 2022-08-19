using System;
using Newtonsoft.Json;

namespace Mosaico.SDK.Relay.Models
{
    public class ClaimParams
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        
        [JsonProperty("network")]
        public string Network { get; set; }
        
        [JsonProperty("stakingPairId")]
        public Guid StakingPairId { get; set; }
    }
}