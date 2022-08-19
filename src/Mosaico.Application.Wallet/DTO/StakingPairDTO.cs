using System;
using System.Collections.Generic;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class StakingPairDTO
    {
        public Guid Id { get; set; }
        public TokenDTO Token { get; set; }
        public TokenDTO StakingToken { get; set; }
        public PaymentCurrencyDTO StakingPaymentCurrency { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StakingPairBaseCurrencyType Type { get; set; }
        public string ContractAddress { get; set; }
        public bool CanChangeStakingPeriod { get; set; }
        public int? MinimumDaysToStake { get; set; }
        public string Network { get; set; }
        public decimal EstimatedAPR { get; set; }
        public decimal EstimatedRewardInUSD { get; set; }
        public List<string> PaymentCurrencies { get; set; }
        public string Version { get; set; }
        public string StakingRegulation { get; set; }
        public string TermsAndConditionsUrl { get; set; }
    }
}