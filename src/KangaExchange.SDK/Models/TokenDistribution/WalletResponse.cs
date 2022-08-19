using System.Collections.Generic;

namespace KangaExchange.SDK.Models.TokenDistribution
{
    public class WalletResponse
    {
        public string Result { get; set; }
        public List<WalletDto> Wallets { get; set; }
        public int? Code { get; set; }
    }
}