using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class VestingWalletDTO
    {
        public Guid Id { get; set; }
        public TokenDTO Token { get; set; }
        public decimal Claimed { get; set; }
        public decimal Locked { get; set; }
        public DateTimeOffset? NextUnlock { get; set; }
        public int TotalPeriod { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
        public bool CanClaim { get; set; }
        public decimal TokensToClaim { get; set; }
    }
}