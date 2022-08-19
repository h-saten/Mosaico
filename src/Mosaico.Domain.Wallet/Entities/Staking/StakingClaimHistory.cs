using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities.Staking
{
    public class StakingClaimHistory : EntityBase
    {
        public Guid StakingPairId { get; set; }
        public virtual StakingPair StakingPair { get; set; }
        public DateTimeOffset ClaimedAt { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionHash { get; set; }
        public string Wallet { get; set; }
    }
}