using System;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;
using Nethereum.RPC.Accounts;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface ICrowdsalePurchaseService
    {
        Task<bool> CanPurchaseAsync(string userId, decimal tokenAmount, Guid projectId, string paymentMethod = null, bool ignoreTransactionCount = false);
        Task<IAccount> GetExecutiveAccountAsync(Transaction transaction);
        Task<string> TransferTargetTokensToUserAsync(Transaction transaction);
        Task AddTokenToUserWalletAsync(Guid tokenId, string walletAddress, string network);
        Task<string> GetContractAddressAsync(Transaction transaction);
        Task<string> RefundPaymentAsync(Transaction transaction, decimal amount);
        Task<string> WithdrawPaymentAsync(Domain.Wallet.Entities.Wallet wallet, Transaction transaction, decimal amount);
    }
}