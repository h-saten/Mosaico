using System;

namespace Mosaico.Domain.Wallet.Entities
{
    public class WalletBalanceSnapshot
    {
        public string WalletAddress { get; set; }
        public string Network { get; set; }
        public decimal Balance { get; set; }
        public DateTimeOffset GeneratedAt { get; set; }
    }
}