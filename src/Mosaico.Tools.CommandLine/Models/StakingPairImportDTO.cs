using System.Collections.Generic;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Tools.CommandLine.Models
{
    public class StakingPairImportDTO
    {
        public string Address { get; set; }
        public bool CanChangeStakingPeriod { get; set; }
        public int MinimumDaysToStake { get; set; }
        public string Network { get; set; }
        public decimal EstimatedAPR { get; set; }
        public decimal EstimatedRewardInUSD { get; set; }
        public List<string> PaymentCurrencies { get; set; }
        public string Token { get; set; }
        public string StakingToken { get; set; }
        public int RewardPayedOnDay { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StakingPairBaseCurrencyType Type { get; set; }
        
        [JsonProperty("cron")]
        public string Cron { get; set; }
        
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("skipApproval")]
        public bool SkipApproval { get; set; }
    }
}