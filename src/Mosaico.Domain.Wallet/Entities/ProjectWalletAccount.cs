using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class ProjectWalletAccount : EntityBase
    {
        public virtual ProjectWallet ProjectWallet { get; set; }
        public Guid ProjectWalletId { get; set; }
        public int AccountId { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
    }
}