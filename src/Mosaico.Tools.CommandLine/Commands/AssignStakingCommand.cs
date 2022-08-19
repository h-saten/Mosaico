using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("assign-staking")]
    public class AssignStakingCommand : CommandBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private Guid _stakingPairId;
        private readonly ILogger _logger;
        private readonly IWalletStakingService _walletStakingService;

        public AssignStakingCommand(IWalletDbContext walletDbContext, ILogger logger, IWalletStakingService walletStakingService)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _walletStakingService = walletStakingService;
            SetOption("-pairId", "Staking pair ID", (s) => _stakingPairId = Guid.Parse(s));
        }

        public override async Task Execute()
        {
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(p => p.Id == _stakingPairId);
            if (pair == null)
            {
                _logger?.Error($"pair {_stakingPairId} not found");
                throw new Exception($"Pair not found");
            }

            var operations = await _walletDbContext.Operations.Where(s => s.Network == pair.Network &&
                                                                          s.State == OperationState.FAILED
                                                                          && s.Type == BlockchainOperationType.STAKING)
                .ToListAsync();

            foreach (var operation in operations)
            {
                _logger?.Information($"Scanning operation {operation.Id}");
                var userWallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == operation.UserId);
                if (userWallet != null)
                {
                    _logger?.Information($"User wallet {userWallet.AccountAddress}");
                    var balance = await _walletStakingService.GetStakingBalanceAsync(pair, userWallet.AccountAddress);
                    if (balance > 0)
                    {
                        _logger?.Information($"User actually staked something and has balance of {balance}");
                        var successStakes = await _walletDbContext.Stakings.Where(t =>
                            t.StakingPairId == _stakingPairId && t.UserId == operation.UserId && t.Status == StakingStatus.Deployed).SumAsync(t => t.Balance);
                        _logger?.Information($"User has {successStakes} stakes in the database");
                        if (balance > successStakes)
                        {
                            _logger?.Information($"Correcting the difference...");
                            _walletDbContext.Stakings.Add(new Staking
                            {
                                Balance = balance - successStakes,
                                Days = 1,
                                Status = StakingStatus.Deployed,
                                StakingPair = pair,
                                StakingPairId = pair.Id,
                                UserId = operation.UserId
                            });
                            await _walletDbContext.SaveChangesAsync();
                            _logger?.Information($"Difference saved");
                        }
                    }
                }
            }

        }
    }
}