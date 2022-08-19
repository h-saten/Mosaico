using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using KangaExchange.SDK.Abstractions;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Repositories;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.PerformWalletSnapshotJob, IsRecurring = true)]
    public class PerformWalletSnapshotJob : HangfireBackgroundJobBase
    {
        private readonly IWalletBalanceSnapshotRepository _snapshotRepository;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserWalletService _userWalletService;

        public PerformWalletSnapshotJob(IWalletBalanceSnapshotRepository snapshotRepository, IWalletDbContext walletDbContext, IUserWalletService userWalletService)
        {
            _snapshotRepository = snapshotRepository;
            _walletDbContext = walletDbContext;
            _userWalletService = userWalletService;
        }
        
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            var skip = 0;
            var take = 1000;
            var delayInMs = 1000;
            var wallets = await _walletDbContext.Wallets
                .Include(w => w.Tokens).ThenInclude(t => t.Token).ThenInclude(t => t.Stakings)
                .AsNoTracking()
                .Skip(skip).Take(take).ToListAsync();

            while (wallets.Any())
            {
                await PerformSnapshotAsync(wallets);
                skip += take;
                wallets = await _walletDbContext.Wallets.AsNoTracking().Skip(skip).Take(take).ToListAsync();
                Thread.Sleep(delayInMs);
            }
        }

        private async Task PerformSnapshotAsync(List<Domain.Wallet.Entities.Wallet> wallets, CancellationToken t = new CancellationToken())
        {
            foreach (var wallet in wallets)
            {
                try
                {
                    var tokens = await _userWalletService.GetTokenBalancesAsync(wallet,  null, t);
                    var accountBalance = tokens.Sum(t => t.TotalAssetValue);
                    await _snapshotRepository.SaveSnapshotAsync(new WalletBalanceSnapshot
                    {
                        Balance = accountBalance,
                        Network = wallet.Network,
                        GeneratedAt = DateTimeOffset.UtcNow,
                        WalletAddress = wallet.AccountAddress
                    });
                }
                catch (InvalidNetworkException ex)
                {
                    continue;
                }
            }
        }
    }
}