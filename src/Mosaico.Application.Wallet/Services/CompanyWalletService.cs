using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit.Util;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Repositories;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Nethereum.RPC.Accounts;

namespace Mosaico.Application.Wallet.Services
{
    public class CompanyWalletService : WalletServiceBase, ICompanyWalletService
    {
        private readonly IBusinessManagementClient _businessManagement;

        public CompanyWalletService(IWalletDbContext walletContext, IAccountRepository accountRepository,
            IWalletBalanceSnapshotRepository snapshotRepository, IEthereumClientFactory ethereumClientFactory,
            ITokenService tokenService, IBusinessManagementClient businessManagement) : base(walletContext, accountRepository, snapshotRepository,
            ethereumClientFactory, tokenService)
        {
            _businessManagement = businessManagement;
        }

        public async Task<List<TokenBalanceDTO>> GetTokenBalancesAsync(CompanyWallet wallet, string tokenTicker = null,
            CancellationToken cancellationToken = new())
        {
            var tokens = wallet.Tokens.Distinct(new CompanyWalletComparer()).Select(t => t.Token)
                .Where(t => t.Status == TokenStatus.Deployed).ToList();
            if (!string.IsNullOrWhiteSpace(tokenTicker)) tokens = tokens.Where(t => t.Symbol == tokenTicker).ToList();
            var tokenBalances =
                await GetTokenBalancesAsync(wallet.AccountAddress, wallet.Network, tokens, cancellationToken);
            return tokenBalances;
        }

        public async Task<CompanyWallet> CreateCompanyWalletAsync(Guid companyId, string network)
        {
            var company = await _businessManagement.GetCompanyAsync(companyId);
            if (company == null) throw new CompanyWalletNotFoundException(companyId.ToString());

            var client = EthereumClientFactory.GetClient(network);
            var account = client.CreateWallet();

            var walletEntity = new CompanyWallet
            {
                CompanyId = companyId,
                Network = network,
                AccountAddress = account.Address,
                PrivateKey = account.PrivateKey,
                PublicKey = account.PublicKey
            };

            WalletContext.CompanyWallets.Add(walletEntity);
            await WalletContext.SaveChangesAsync();
            return walletEntity;
        }

        public override async Task AddTokenToWalletAsync(string address, string contractAddress, string network)
        {
            var wallet =
                await WalletContext.CompanyWallets.FirstOrDefaultAsync(w =>
                    w.AccountAddress == address && w.Network == network);
            if (wallet != null && !wallet.Tokens.Any(t => t.Token.Address == contractAddress))
            {
                var token = await WalletContext.Tokens.FirstOrDefaultAsync(t =>
                    t.Address == contractAddress && t.Network == network);
                if (token != null)
                {
                    wallet.Tokens.Add(new CompanyWalletToToken
                    {
                        Token = token,
                        TokenId = token.Id,
                        CompanyWallet = wallet,
                        CompanyWalletId = wallet.Id
                    });
                    await WalletContext.SaveChangesAsync();
                }
            }
        }

        public async Task<IAccount> GetAccountAsync(Guid companyId, string network)
        {
            var companyWallet =
                await WalletContext.CompanyWallets.FirstOrDefaultAsync(c => c.CompanyId == companyId && c.Network == network);
            if(companyWallet == null) throw new CompanyWalletNotFoundException(companyId, network);
            var client = EthereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(companyWallet.PrivateKey);
            return account;
        }
    }
}