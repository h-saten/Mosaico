using System;
using System.Collections.Generic;

namespace Mosaico.Domain.Wallet.Entities
{
    public class CompanyWalletComparer : IEqualityComparer<CompanyWalletToToken>
    {
        public bool Equals(CompanyWalletToToken x, CompanyWalletToToken y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.TokenId.Equals(y.TokenId) && x.CompanyWalletId.Equals(y.CompanyWalletId);
        }

        public int GetHashCode(CompanyWalletToToken obj)
        {
            return HashCode.Combine(obj.TokenId, obj.CompanyWalletId);
        }
    }
    public class CompanyWalletToToken
    {
        public Guid TokenId { get; set; }
        public virtual Token Token { get; set; }
        public Guid CompanyWalletId { get; set; }
        public virtual CompanyWallet CompanyWallet { get; set; }
    }
}