using System.Threading.Tasks;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.Models.TokenDistribution;

namespace KangaExchange.SDK.Abstractions
{
    public interface IKangaTokenDistributionApiClient
    {
        Task<StandardResponse> Transfer(string from, string to, string currency, decimal quantity);
        Task<WalletResponse> WalletBalance(string walletName = null);
        Task<decimal> WalletBalanceForToken(string tokenTicker, string walletName = null);
        Task<StandardResponse> WalletShift(string fromWalletName, string toWalletName, string currency, decimal quantity);
        Task<StandardResponse> CreateWallet(string name);
    }
}