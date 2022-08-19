using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class WalletVestingDTO
    {
        public Guid VestingId { get; set; }
        public TokenBalanceDTO Token { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public long Days { get; set; }
    }
}