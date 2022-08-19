using System;
using System.Threading.Tasks;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("import-staking")]
    public class ImportStakingCommand : CommandBase
    {
        private readonly IWalletDbContext _dbContext;

        public ImportStakingCommand(IWalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task Execute()
        {
            var stakingPairId = Guid.Parse("46CB95E2-E3A6-4496-57D8-08DA5DBA1BCF");
            var balance = 1m;
            var userId = "d2308193-87e0-43a9-a499-474a64d1f40e";

            var staking = new Staking
            {
                Days = 0,
                StakingPairId = stakingPairId,
                Status = StakingStatus.Deployed,
                Balance = balance,
                UserId = userId,
                WalletType = StakingWallet.METAMASK,
                CreatedAt = DateTimeOffset.Parse("2022-07-06T17:00:00")
            };
            _dbContext.Stakings.Add(staking);
            await _dbContext.SaveChangesAsync();
        }
    }
}