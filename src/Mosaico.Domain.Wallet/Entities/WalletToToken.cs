using System;
using System.Collections.Generic;

namespace Mosaico.Domain.Wallet.Entities
{
    public class WalletToTokenComparer : IEqualityComparer<WalletToToken>
    {
        public bool Equals(WalletToToken x, WalletToToken y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.TokenId.Equals(y.TokenId) && x.WalletId.Equals(y.WalletId);
        }

        public int GetHashCode(WalletToToken obj)
        {
            return HashCode.Combine(obj.TokenId, obj.WalletId);
        }
    }
    
    public class WalletToToken
    {
        public Guid TokenId { get; set; }
        public virtual Token Token { get; set; }
        public Guid WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public decimal Balance { get; set; }
    }
}