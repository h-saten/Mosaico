using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class Vault : EntityBase
    {
        public string Address { get; set; }
        public Guid CompanyId { get; set; }
        public string Network { get; set; }
        public Guid TokenId { get; set; }
        public virtual Token Token { get; set; }
        public virtual List<TokenDistribution> TokenDistributions { get; set; } = new List<TokenDistribution>();
    }
}