using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class WalletToVesting : EntityBase
    {
        public Guid WalletId { get; set; }
        public Guid VestingId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public virtual Vesting Vesting { get; set; }
    }
}