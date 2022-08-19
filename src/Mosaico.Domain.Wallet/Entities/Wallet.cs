using System;
using System.Collections.Generic;
using System.IO;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class Wallet : EntityBase, IWallet
    {
        public string UserId { get; set; }
        
        [Encrypted]
        public string PrivateKey { get; set; }
        public string AccountAddress { get; set; }
        public string Network { get; set; }
        
        [Encrypted]
        public string PublicKey { get; set; }
        public virtual List<WalletToToken> Tokens { get; set; } = new List<WalletToToken>();
        public virtual List<WalletToVesting> Vestings { get; set; } = new List<WalletToVesting>();
        
        public string LastSyncBlockHash { get; set; }
    }
}