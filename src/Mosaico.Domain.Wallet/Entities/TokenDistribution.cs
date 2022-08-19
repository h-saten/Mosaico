using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class TokenDistribution : EntityBase
    {
        public string Name { get; set; }
        public Guid TokenId { get; set; }
        public virtual Token Token { get; set; }
        public decimal TokenAmount { get; set; }
        public string SmartContractId { get; set; }
        public Guid? VaultId { get; set; }
        public virtual Vault Vault { get; set; }
    }
}