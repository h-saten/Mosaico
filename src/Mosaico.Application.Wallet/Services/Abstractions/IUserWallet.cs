using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.ValueObjects;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IUserWalletService
    {
        Task<Domain.Wallet.Entities.Wallet> GetWalletAsync(string userId, string network);
        Task<List<TokenBalanceDTO>> GetTokenBalancesAsync(Domain.Wallet.Entities.Wallet wallet, string tokenTicker = null,
            CancellationToken cancellationToken = new CancellationToken());
        
        Task<TokenBalanceDTO> GetTokenBalanceAsync(Domain.Wallet.Entities.Wallet wallet, Token token,
            CancellationToken cancellationToken = new CancellationToken());

        Task<decimal?> GetPreviousBalanceAsync(string walletAddress, string network, TimeSpan timePeriod);
        Task<Domain.Wallet.Entities.Wallet> CreateWalletAsync(string userId, string network);

        Task<decimal> PaymentCurrencyBalanceAsync(
            string walletAddress,
            string paymentCurrencyTicker,
            string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum);

        // Task<Dictionary<Guid, decimal>> GetCrowdSaleTokenBalancesAsync(Domain.Wallet.Entities.Wallet wallet,
        //     CancellationToken cancellationToken = new());

        Task<decimal> NativePaymentCurrencyBalanceAsync(string walletAddress, string paymentCurrencyTicker,
            string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum);
        
        Task AddTokenToWalletAsync(string address, string contractAddress, string network);
        Task<string> TransferTokenAsync(IWallet wallet, string contractAddress, string recipient, decimal amount);
        Task<string> TransferNativeCurrencyAsync(IWallet wallet, string recipient, decimal amount);

        Task<TokenBalanceDTO> GetCurrencyBalanceAsync(Domain.Wallet.Entities.Wallet wallet, string ticker,
            string chain);
    }
}