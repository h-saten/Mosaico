using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class TokenStakingDTO
    {
        public Guid StakingId { get; set; }
        public Guid WalletId { get; set; }
        public decimal Quantity { get; set; }
        public DateTimeOffset StakedAt { get; set; }
    }
}