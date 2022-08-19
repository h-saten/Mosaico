using System;
using System.Collections.Generic;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class ProjectWallet : EntityBase
    {
        [Encrypted]
        public string Mnemonic { get; set; }
        
        [Encrypted]
        public string Password { get; set; }
        public string Network { get; set; }
        public Guid ProjectId { get; set; }
        public virtual List<ProjectWalletAccount> Accounts { get; set; } = new List<ProjectWalletAccount>();
    }
}