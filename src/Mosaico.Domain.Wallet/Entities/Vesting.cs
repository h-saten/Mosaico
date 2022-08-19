using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class Vesting : EntityBase
    {
        public string Name { get; set; }
        public decimal TokenAmount { get; set; }
        public int NumberOfDays { get; set; }
        public string WalletAddress { get; set; }
        public decimal InitialPaymentPercentage { get; set; }
        public virtual List<VestingFund> Funds { get; set; } = new List<VestingFund>();
        public Guid TokenId { get; set; }
        public virtual Token Token { get; set; }
        public Guid? VaultId { get; set; }
        public virtual Vault Vault { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
    }
}