using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class WalletBalanceHistoryDTO
    {
        public DateTimeOffset Date { get; set; }
        public decimal Balance { get; set; }
    }
}