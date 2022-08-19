using System;
using System.Collections.Generic;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class CompanyWallet : EntityBase, IWallet
    {
        public Guid CompanyId { get; set; }
        public string AccountAddress { get; set; }
        public string Network { get; set; }
        public virtual List<CompanyWalletToToken> Tokens { get; set; } = new List<CompanyWalletToToken>();
        public string LastSyncBlockHash { get; set; }
        
        [Encrypted]
        public string PrivateKey { get; set; }
        
        [Encrypted]
        public string PublicKey { get; set; }
    }
}