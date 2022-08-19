using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.GetWalletBalanceHistory
{
    public class GetWalletBalanceHistoryQueryResponse
    {
        public List<WalletBalanceHistoryDTO> Balances { get; set; } = new List<WalletBalanceHistoryDTO>();
    }
}