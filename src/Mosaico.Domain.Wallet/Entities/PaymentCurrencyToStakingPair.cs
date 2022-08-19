using System;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Domain.Wallet.Entities
{
    public class PaymentCurrencyToStakingPair : EntityBase
    {
        public virtual StakingPair StakingPair { get; set; }
        public Guid StakingPairId { get; set; }
        public virtual PaymentCurrency PaymentCurrency { get; set; }
        public Guid PaymentCurrencyId { get; set; }
    }
}