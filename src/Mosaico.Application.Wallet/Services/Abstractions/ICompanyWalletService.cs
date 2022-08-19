using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Entities;
using Nethereum.RPC.Accounts;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface ICompanyWalletService
    {
        Task<CompanyWallet> CreateCompanyWalletAsync(Guid companyId, string network);
        Task<List<TokenBalanceDTO>> GetTokenBalancesAsync(CompanyWallet wallet, string tokenTicker = null, CancellationToken cancellationToken = new());
        Task<decimal?> GetPreviousBalanceAsync(string walletAddress, string network, TimeSpan timePeriod);

        Task<decimal> NativePaymentCurrencyBalanceAsync(string walletAddress, string paymentCurrencyTicker,
            string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum);

        Task<decimal> PaymentCurrencyBalanceAsync(
            string walletAddress,
            string paymentCurrencyTicker,
            string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum);

        Task AddTokenToWalletAsync(string address, string contractAddress, string network);
        Task<string> TransferTokenAsync(IWallet wallet, string contractAddress, string recipient, decimal amount);
        Task<string> TransferNativeCurrencyAsync(IWallet wallet, string recipient, decimal amount);
        Task<IAccount> GetAccountAsync(Guid companyId, string network);

        Task<string> TransferTokenWithoutReceiptAsync(IWallet wallet, string contractAddress, string recipient,
            decimal amount);
    }
}