using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Extensions;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Repositories;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Application.Wallet.Services
{
    public class UserWalletService : WalletServiceBase, IUserWalletService
    {
        private readonly ITokenLockService _lockService;
        
        public UserWalletService(IWalletDbContext walletContext, IAccountRepository accountRepository,
            IWalletBalanceSnapshotRepository snapshotRepository, IEthereumClientFactory ethereumClientFactory,
            ITokenService tokenService, ITokenLockService lockService) : base(walletContext, accountRepository, snapshotRepository,
            ethereumClientFactory, tokenService)
        {
            _lockService = lockService;
        }
        
        public Task<Domain.Wallet.Entities.Wallet> GetWalletAsync(string userId, string network)
        {
            return WalletContext.Wallets.FirstOrDefaultAsync(w => w.Network == network && w.UserId == userId);
        }

        public async Task<TokenBalanceDTO> GetCurrencyBalanceAsync(Domain.Wallet.Entities.Wallet wallet, string ticker,
            string chain)
        {
            var paymentCurrency = await WalletContext
                .PaymentCurrencies
                .FirstOrDefaultAsync(p => p.Ticker == ticker && p.Chain == chain);
            
            if (paymentCurrency == null)
            {
                throw new TokenNotFoundException($"Token not found");
            }

            decimal balance;
            if (paymentCurrency.NativeChainCurrency)
            {
                balance =
                    await NativePaymentCurrencyBalanceAsync(wallet.AccountAddress,
                        paymentCurrency.Ticker, paymentCurrency.Chain);
            }
            else
            {
                balance = await PaymentCurrencyBalanceAsync(wallet.AccountAddress, paymentCurrency.Ticker, paymentCurrency.Chain);
            }
            var tokenBalance = new TokenBalanceDTO
            {
                Balance = balance.TruncateDecimals(),
                Name = paymentCurrency.Name,
                Id = paymentCurrency.Id,
                Symbol = paymentCurrency.Ticker,
                ContractAddress = paymentCurrency.ContractAddress,
                LogoUrl = paymentCurrency.LogoUrl,
                IsPaymentCurrency = true,
                Network = paymentCurrency.Chain,
                Currency = Constants.FIATCurrencies.USD
            };
            var locks = await _lockService.GetCurrencyLockSumAsync(paymentCurrency.Id, wallet.UserId);
            tokenBalance.Balance -= locks;
            if (tokenBalance.Balance < 0) tokenBalance.Balance = 0;
            var rate = await WalletContext.ExchangeRates.OrderByDescending(r => r.CreatedAt)
                            .FirstOrDefaultAsync(er => er.Ticker == paymentCurrency.Ticker);
            tokenBalance.TotalAssetValue = tokenBalance.Balance * rate?.Rate ?? 0;
            return tokenBalance;
        }

        public async Task<List<TokenBalanceDTO>> GetTokenBalancesAsync(Domain.Wallet.Entities.Wallet wallet,
            string tokenTicker = null,
            CancellationToken cancellationToken = new())
        {
            var tokens = wallet.Tokens.Distinct(new WalletToTokenComparer()).Where(x => x.Token.Address != null).Select(t => t.Token).ToList();
            var tokenIds = tokens.Select(t => t.Id);
            
            var displayAlwaysTokens = await WalletContext.Tokens.Where(t => t.DisplayAlways == true && !tokenIds.Contains(t.Id)).ToListAsync(cancellationToken: cancellationToken);
            tokens = tokens.Union(displayAlwaysTokens).ToList();
            
            if (!string.IsNullOrWhiteSpace(tokenTicker)) tokens = tokens.Where(t => t.Symbol == tokenTicker).ToList();

            var tokenBalances =
                await GetTokenBalancesAsync(wallet.AccountAddress, wallet.Network, tokens, cancellationToken);
            return tokenBalances;
        }

        public async Task<TokenBalanceDTO> GetTokenBalanceAsync(Domain.Wallet.Entities.Wallet wallet, Token token,
            CancellationToken cancellationToken = new())
        {
            var tokenBalance =
                await GetTokenBalanceAsync(wallet.AccountAddress, wallet.Network, token, cancellationToken);
            var locks = await _lockService.GetTokenLockSumAsync(token.Id, wallet.UserId, cancellationToken);
            tokenBalance.Balance -= locks;
            if (tokenBalance.Balance < 0) tokenBalance.Balance = 0;
            var rate = await WalletContext.ExchangeRates.OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync(er => er.Ticker == token.Symbol, cancellationToken: cancellationToken);
            tokenBalance.TotalAssetValue = tokenBalance.Balance * rate?.Rate ?? 0;
            return tokenBalance;
        }

        public async Task<Domain.Wallet.Entities.Wallet> CreateWalletAsync(string userId, string network)
        {
            var client = EthereumClientFactory.GetClient(network);
            var account = client.CreateWallet();

            var walletEntity = new Domain.Wallet.Entities.Wallet
            {
                UserId = userId,
                Network = network,
                PrivateKey = account.PrivateKey,
                AccountAddress = account.Address,
                PublicKey = account.PublicKey
            };

            WalletContext.Wallets.Add(walletEntity);
            await WalletContext.SaveChangesAsync();
            return walletEntity;
        }

        public override async Task AddTokenToWalletAsync(string address, string contractAddress, string network)
        {
            var wallet =
                await WalletContext.Wallets.FirstOrDefaultAsync(
                    w => w.AccountAddress == address && w.Network == network);
            if (wallet != null && !wallet.Tokens.Any(t => t.Token.Address == contractAddress))
            {
                var token = await WalletContext.Tokens.FirstOrDefaultAsync(t =>
                    t.Address == contractAddress && t.Network == network);
                if (token != null)
                {
                    wallet.Tokens.Add(new WalletToToken
                    {
                        Token = token,
                        TokenId = token.Id,
                        Wallet = wallet,
                        WalletId = wallet.Id
                    });
                    await WalletContext.SaveChangesAsync();
                }
            }
        }
    }
}