using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.SDK.Wallet.Models;

namespace Mosaico.SDK.Wallet.Abstractions
{
    public interface IWalletClient
    {
        Task<TokenWallet> TokenWalletDetails(Guid tokenId, DateTimeOffset? fromDate, DateTimeOffset? toDate);
        Task<TokenWallet> StageTransactionsDetails(Guid tokenId, Guid stageId);
        Task<MosaicoToken> GetTokenAsync(Guid id);
        Task UpdateTokenAsync(MosaicoToken tokenDTO);
        Task<MosaicoToken> GetTokenAsync(string name);
        Task<bool> IsInvestorAsync(Guid projectId, string userId);
        Task<MosaicoCompanyWallet> GetCompanyWalletAsync(Guid companyId, string network);

        Task MintTokensToCompanyWallet(Guid companyId, Guid tokenId);
        Task<MosaicoToken> CreateTokenAsync(string name, string symbol, long totalSupply, string network, bool isMintable, bool isBurnable, string type);
        Task SetTokenDeployedAsync(Guid id, string ownerAddress, string contractAddress, string version);
        Task<int?> GetNumberOfBuyersPerToken(Guid tokenId);
        Task<Tuple<decimal?, decimal?>> GetTokenRaisedAmountAsync(Guid tokenId, Guid? projectId = null);
        Task<string> GetPaymentCurrencyAddressAsync(string tokenSymbol, string network);
        Task<List<Guid>> GetTokensWithExchangeAsync();
        Task<InvestorTokenSummary> GetTokenSummaryAsync(Guid tokenId, Guid userId);
        Task<List<MosaicoTokenDistribution>> GetTokenDistributionAsync(Guid id);
        Task<List<string>> GetPaymentCurrencyAddressesAsync(string chain);
        Task<List<PaymentCurrency>> GetPaymentCurrenciesAsync(string chain);
        Task<MosaicoUserWallet> GetUserWalletAsync(string userId, string network);
        Task<TransactionDetails> GetTransactionAsync(Guid transactionId);
        Task<decimal> ExchangeRateAsync(string ticker);
        Task<int> GetProjectNFTCountAsync(Guid projectId);
    }
}