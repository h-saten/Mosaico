using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Base.Extensions;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Repositories;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public abstract class WalletServiceBase
    {
        protected readonly IWalletDbContext WalletContext;
        protected readonly IAccountRepository AccountRepository;
        protected readonly IWalletBalanceSnapshotRepository SnapshotRepository;
        protected readonly IEthereumClientFactory EthereumClientFactory;
        protected readonly ITokenService TokenService;
        
        protected WalletServiceBase(IWalletDbContext walletContext, IAccountRepository accountRepository, IWalletBalanceSnapshotRepository snapshotRepository, IEthereumClientFactory ethereumClientFactory, ITokenService tokenService)
        {
            WalletContext = walletContext;
            AccountRepository = accountRepository;
            SnapshotRepository = snapshotRepository;
            EthereumClientFactory = ethereumClientFactory;
            TokenService = tokenService;
        }
        
        public virtual async Task<decimal?> GetPreviousBalanceAsync(string walletAddress, string network, TimeSpan timePeriod)
        {
            var from = DateTimeOffset.UtcNow.Date.AddDays(-timePeriod.Days);
            var to = DateTimeOffset.UtcNow.Date;
            var results = await SnapshotRepository.GetHistoricalSnapshotsAsync(network, walletAddress, from, to);
            return results.Any() ? results.Average(r => r.Balance.TruncateDecimals()) : null;
        }
        
        public virtual async Task<decimal> NativePaymentCurrencyBalanceAsync(string walletAddress, string paymentCurrencyTicker, string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var paymentCurrency = await WalletContext
                .PaymentCurrencies
                .Where(c => c.Ticker == paymentCurrencyTicker && c.Chain == chain && c.NativeChainCurrency == true)
                .SingleOrDefaultAsync();
            
            if (paymentCurrency is null)
            {
                throw new UnsupportedCurrencyException(paymentCurrencyTicker);
            }
            
            var balance = await AccountRepository.AccountBalanceAsync(walletAddress, chain);
            return balance?.Balance.TruncateDecimals() ?? 0;
        }
        
        public virtual async Task<decimal> PaymentCurrencyBalanceAsync(
            string walletAddress, 
            string paymentCurrencyTicker,
            string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var paymentCurrency = await WalletContext
                .PaymentCurrencies
                .Where(c => c.Ticker == paymentCurrencyTicker && c.Chain == chain && !c.NativeChainCurrency)
                .SingleOrDefaultAsync();
            
            if (paymentCurrency is null)
            {
                throw new UnsupportedCurrencyException(paymentCurrencyTicker);
            }

            if (string.IsNullOrWhiteSpace(paymentCurrency.ContractAddress))
            {
                throw new UnsupportedChainException(chain);
            }
            
            var userBalanceInPaymentCurrency = await AccountRepository
                .Erc20BalanceAsync(walletAddress, paymentCurrency.ContractAddress, chain);
            
            return new Wei(userBalanceInPaymentCurrency, paymentCurrency.DecimalPlaces).ToDecimal().TruncateDecimals();
        }

        protected virtual async Task<List<TokenBalanceDTO>> GetTokenBalancesAsync(string address, string network, List<Token> tokens, CancellationToken cancellationToken = new CancellationToken())
        {
            var paymentCurrencies = await WalletContext
                .PaymentCurrencies
                .AsNoTracking()
                .Where(x => x.Chain == network)
                .OrderByDescending(x => x.NativeChainCurrency)
                .ToListAsync(cancellationToken);
            
            var balances = new List<TokenBalanceDTO>();

            foreach (var pc in paymentCurrencies)
            {
                var balance = !pc.NativeChainCurrency ?
                    await PaymentCurrencyBalanceAsync(address, pc.Ticker,
                        network)
                    : await NativePaymentCurrencyBalanceAsync(address, pc.Ticker, network);

                var rate = await WalletContext.ExchangeRates.OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefaultAsync(er => er.Ticker == pc.Ticker, cancellationToken: cancellationToken);
                var totalAssetValue = balance * rate?.Rate ?? 0;
                
                balances.Add(new TokenBalanceDTO
                {
                    Id = pc.Id,
                    Balance = balance.TruncateDecimals(),
                    Name = pc.Name,
                    Symbol = pc.Ticker,
                    ContractAddress = pc.ContractAddress,
                    LogoUrl = pc.LogoUrl,
                    IsStakable = false,
                    IsExchangable = false,
                    Network = pc.Chain,
                    IsPaymentCurrency = true,
                    Currency = Constants.FIATCurrencies.USD,
                    TotalAssetValue = totalAssetValue.TruncateDecimals()
                });
            }

            foreach (var token in tokens)
            {
                var balanceRaw = await AccountRepository.Erc20BalanceAsync(address, token.Address, network, token.GetERCType());
                var balance = new Wei(balanceRaw).ToDecimal().TruncateDecimals();
                
                var rate = await WalletContext.ExchangeRates.OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefaultAsync(er => er.Ticker == token.Symbol, cancellationToken: cancellationToken);
                var totalAssetValue = balance * rate?.Rate ?? 0;
                
                balances.Add(new TokenBalanceDTO
                {
                    Balance = balance.TruncateDecimals(),
                    Name = token.Name,
                    Id = token.Id,
                    Symbol = token.Symbol,
                    ContractAddress = token.Address,
                    LogoUrl = token.LogoUrl,
                    IsStakable = token.Stakings?.Any() == true,
                    IsExchangable = false,
                    IsPaymentCurrency = false,
                    Network = token.Network,
                    Currency = Constants.FIATCurrencies.USD,
                    TotalAssetValue = totalAssetValue.TruncateDecimals()
                });
            }

            return balances;
        }

        protected virtual async Task<TokenBalanceDTO> GetTokenBalanceAsync(string address, string network, Token token,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var balanceRaw = await AccountRepository.Erc20BalanceAsync(address, token.Address, network, token.GetERCType());
            var balance = new Wei(balanceRaw).ToDecimal().TruncateDecimals();
            
            return new TokenBalanceDTO
            {
                Balance = balance,
                Name = token.Name,
                Id = token.Id,
                Symbol = token.Symbol,
                ContractAddress = token.Address,
                LogoUrl = token.LogoUrl,
                IsStakable = token.Stakings?.Any() == true,
                IsExchangable = false,
                IsPaymentCurrency = false,
                Network = token.Network,
                Currency = Constants.FIATCurrencies.USD
            };
        }
        
        public async Task<string> TransferTokenAsync(IWallet wallet, string contractAddress, string recipient, decimal amount)
        {
            var client = EthereumClientFactory.GetClient(wallet.Network);
            var account = await client.GetAccountAsync(wallet.PrivateKey);
            if (account == null) throw new WalletNotFoundException(wallet.AccountAddress);
            if (string.IsNullOrWhiteSpace(contractAddress)) throw new UnsupportedCurrencyException(contractAddress);
            var transactionHash = await TokenService.TransferAsync(wallet.Network, account,
                contractAddress, recipient, amount);
            return transactionHash;
        }
        
        public async Task<string> TransferTokenWithoutReceiptAsync(IWallet wallet, string contractAddress, string recipient, decimal amount)
        {
            var client = EthereumClientFactory.GetClient(wallet.Network);
            var account = await client.GetAccountAsync(wallet.PrivateKey);
            if (account == null) throw new WalletNotFoundException(wallet.AccountAddress);
            if (string.IsNullOrWhiteSpace(contractAddress)) throw new UnsupportedCurrencyException(contractAddress);
            var transactionHash = await TokenService.TransferWithoutReceiptAsync(wallet.Network, account,
                contractAddress, recipient, amount);
            return transactionHash;
        }
        
        public async Task<string> TransferNativeCurrencyAsync(IWallet wallet, string recipient, decimal amount)
        {
            var client = EthereumClientFactory.GetClient(wallet.Network);
            var account = await client.GetAccountAsync(wallet.PrivateKey);
            if (account == null) throw new WalletNotFoundException(wallet.AccountAddress);
            var transactionHash = await client.TransferFundsAsync(account, recipient, amount);
            return transactionHash;
        }

        public abstract Task AddTokenToWalletAsync(string address, string contractAddress, string network);
        
    }
}