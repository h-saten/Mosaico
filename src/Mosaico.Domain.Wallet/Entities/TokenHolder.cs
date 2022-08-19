using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class TokenHolder : EntityBase
    {
        public string WalletAddress { get; set; }
        public decimal Balance { get; set; }
        public virtual Token Token { get; set; }
        public Guid TokenId { get; set; }
    }
}