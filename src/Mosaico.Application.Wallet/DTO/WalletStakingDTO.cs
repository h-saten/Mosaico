using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class WalletStakingDTO
    {
        public Guid StakingId { get; set; }
        public Guid WalletId { get; set; }
        public Guid TokenId { get; set; }
        public decimal Quantity { get; set; }
        public string TokenSymbol { get; set; }
    }
}