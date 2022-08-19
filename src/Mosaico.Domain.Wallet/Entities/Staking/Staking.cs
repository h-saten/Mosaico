using System;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Domain.Wallet.Entities.Staking
{
    public class Staking : EntityBase
    {
        public Guid StakingPairId { get; set; }
        public virtual StakingPair StakingPair { get; set; }
        public decimal Balance { get; set; }
        public int Days { get; set; }
        public StakingStatus Status { get; set; }
        public string FailureReason { get; set; }
        public string UserId { get; set; }
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
        public StakingWallet WalletType { get; set; }

        public bool CanDeploy()
        {
            return Status == StakingStatus.Pending;
        }
    }
}