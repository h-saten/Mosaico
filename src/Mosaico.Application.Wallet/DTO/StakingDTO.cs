using System;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class StakingDTO
    {
        public Guid Id { get; set; }
        public TokenDTO Token { get; set; }
        public DateTimeOffset? NextRewardAt { get; set; }
        public decimal Balance { get; set; }
        public decimal EstimatedAPR { get; set; }
        public decimal EstimatedRewardInUSD { get; set; }
        public int Days { get; set; }
        public string Version { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StakingWallet WalletType { get; set; }
        public string Wallet { get; set; }
        public string ContractAddress { get; set; }
        public string StakingRegulation { get; set; }
        public string TermsAndConditionsUrl { get; set; }
    }
}