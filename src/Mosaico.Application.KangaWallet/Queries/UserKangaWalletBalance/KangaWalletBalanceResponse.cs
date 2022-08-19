using System.Collections.Generic;
using Mosaico.Application.KangaWallet.DTOs;

namespace Mosaico.Application.KangaWallet.Queries.UserKangaWalletBalance
{
    public class KangaWalletBalanceResponse
    {
        public string Currency { get; set; }
        public List<KangaAssetDto> Assets { get; set; }
        public decimal TotalWalletValue { get; set; }
    }
}