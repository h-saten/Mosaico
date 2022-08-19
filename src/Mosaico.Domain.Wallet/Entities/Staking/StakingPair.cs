using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Domain.Wallet.Entities.Staking
{
    public class StakingPair : EntityBase
    {
        public Guid TokenId { get; set; }
        public virtual Token Token { get; set; }
        public Guid? StakingTokenId { get; set; }
        public virtual Token StakingToken { get; set; }
        public Guid? StakingPaymentCurrencyId { get; set; }
        public virtual PaymentCurrency StakingPaymentCurrency { get; set; }
        public StakingPairBaseCurrencyType Type { get; set; }
        public string ContractAddress { get; set; }
        public bool CanChangeStakingPeriod { get; set; }
        public int? MinimumDaysToStake { get; set; }
        public string Network { get; set; }
        public virtual List<PaymentCurrencyToStakingPair> PaymentCurrencies { get; set; } = new List<PaymentCurrencyToStakingPair>();
        public decimal EstimatedAPR { get; set; }
        public decimal EstimatedRewardInUSD { get; set; }
        public bool IsEnabled { get; set; }
        public int RewardPayedOnDay { get; set; }
        public string CronSchedule { get; set; }
        public RewardEstimateCalculatorType CalculatorType { get; set; }
        public bool SkipApproval { get; set; }
        public bool IsWithdrawalDisabled { get; set; }
        public string StakingVersion { get; set; }
        public Guid? StakingRegulationId { get; set; }
        public virtual StakingRegulation StakingRegulation { get; set; }
        public Guid? StakingTermsId { get; set; }
        public virtual StakingTerms StakingTerms { get; set; }
    }
}