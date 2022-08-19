using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using NBitcoin;

namespace Mosaico.Application.Wallet.Services
{
    public class ProjectWalletService : IProjectWalletService
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IStringGenerator _stringGenerator;
        private const int MaxAttempts = 10;

        public ProjectWalletService(IEthereumClientFactory ethereumClientFactory, IWalletDbContext walletDbContext, IStringGenerator stringGenerator)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _walletDbContext = walletDbContext;
            _stringGenerator = stringGenerator;
        }

        public async Task<ProjectWallet> CreateWalletAsync(string network, Guid projectId)
        {
            if (_walletDbContext.ProjectWallets.AsQueryable()
                .Any(pw => pw.ProjectId == projectId && pw.Network == network))
            {
                throw new ProjectWalletAlreadyExistsException(network, projectId);
            }

            var client = _ethereumClientFactory.GetClient(network);
            var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve).ToString();
            var password = _stringGenerator.Generate();
            var wallet = client.CreateHDWallet(mnemonic, password);
            var projectWallet = new ProjectWallet
            {
                Mnemonic = mnemonic,
                Password = password,
                Network = network,
                ProjectId = projectId
            };
            _walletDbContext.ProjectWallets.Add(projectWallet);
            await _walletDbContext.SaveChangesAsync();
            return projectWallet;
        }

        public async Task<ProjectWalletAccount> GetAccountAsync(string network, Guid projectId, string userId)
        {
            var wallet =
                await _walletDbContext.ProjectWallets.FirstOrDefaultAsync(pw =>
                    pw.Network == network && pw.ProjectId == projectId);
            
            if(wallet == null)
            {
                throw new ProjectWalletNotFoundException(network, projectId);
            }

            var account = wallet.Accounts.FirstOrDefault(a => a.UserId == userId);
            if (account == null)
            {
                account = await CreateAccountAsync(wallet, userId);
            }
            return account;
        }

        private async Task<ProjectWalletAccount> CreateAccountAsync(ProjectWallet wallet, string userId)
        {
            using (var t = _walletDbContext.BeginTransaction())
            {
                try
                {
                    var client = _ethereumClientFactory.GetClient(wallet.Network);
                    var accountId = GetAccountId();
                    var attempts = 0;
                    while (_walletDbContext.ProjectWalletAccounts.Any(pwa => pwa.AccountId == accountId))
                    {
                        if (attempts > MaxAttempts)
                        {
                            throw new Exception($"Couldn't generate account for the user {userId}");
                        }

                        accountId = GetAccountId();
                        attempts++;
                    }

                    var bAccount = client.GetWalletAccount(wallet.Mnemonic, wallet.Password, accountId);
                    var walletAccount = new ProjectWalletAccount
                    {
                        Address = bAccount.Address,
                        AccountId = accountId,
                        ProjectWallet = wallet,
                        ProjectWalletId = wallet.Id,
                        UserId = userId
                    };
                    wallet.Accounts.Add(walletAccount);
                    await _walletDbContext.SaveChangesAsync();
                    await t.CommitAsync();
                    return walletAccount;
                }
                catch (Exception)
                {
                    await t.RollbackAsync();
                    throw;
                }
            }
        }

        private int GetAccountId()
        {
            var now = DateTime.UtcNow;
            var zeroDate = DateTime.MinValue.AddHours(now.Hour)
                .AddMinutes(now.Minute)
                .AddSeconds(now.Second)
                .AddMilliseconds(now.Millisecond);
            return (int)(zeroDate.Ticks / 10000);
        }
    }
}