using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class StakingWithdrawalDTO
    {
        public Guid StakingId { get; set; }
        public Guid WalletId { get; set; }
        public decimal Quantity { get; set; }
        public string TokenSymbol { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}