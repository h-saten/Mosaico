using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable;
using Nethereum.Web3;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("clean-staking")]
    public class CleanStakingCommand : CommandBase
    {
        private readonly IWalletDbContext _dbContext;
        private readonly IEthereumClientFactory _clientFactory;
        private readonly ILogger _logger;
        private Guid _stakingPairId;
        private string _network;

        public CleanStakingCommand(IWalletDbContext dbContext, ILogger logger, IEthereumClientFactory clientFactory)
        {
            _dbContext = dbContext;
            _logger = logger;
            _clientFactory = clientFactory;
            SetOption("-stakingPairId", "id of staking pair", (s) => _stakingPairId = Guid.Parse(s));
            SetOption("-network", "network", (s) => _network = s);
        }

        public override async Task Execute()
        {
            var stakingPair = await _dbContext.StakingPairs.FirstOrDefaultAsync(t => t.Id == _stakingPairId && t.Network == _network);
            if (stakingPair == null) throw new Exception($"Staking pair not found");

            var stakings = await _dbContext.Stakings.Where(t => t.StakingPairId == stakingPair.Id && t.Status == StakingStatus.Deployed).ToListAsync();
            var serviceClient = _clientFactory.GetServiceFactory(_network);
            var contract =  await serviceClient.GetServiceAsync<StakingUpgradableService>(stakingPair.ContractAddress, string.Empty);

            foreach (var stake in stakings)
            {
                _logger.Information($"Verifying {stake.Id} for user {stake.UserId}");
                var walletAddress = string.Empty;
                if (stake.WalletType == StakingWallet.METAMASK)
                {
                    walletAddress = stake.Wallet;
                    _logger.Information($"Metamask wallet {walletAddress}");
                }
                else
                {
                    var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(t => t.UserId == stake.UserId && t.Network == _network);
                    if (wallet != null)
                    {
                        walletAddress = wallet.AccountAddress;
                        _logger.Information($"Mosaico wallet {walletAddress}");
                    }
                }

                if (!string.IsNullOrWhiteSpace(walletAddress))
                {
                    var balanceResponse = await contract.BalanceOfQueryAsync(walletAddress);
                    var balance = Web3.Convert.FromWei(balanceResponse);
                    if (balance <= 0)
                    {
                        _logger.Warning(
                            $"User {stake.UserId} has negative balance on smart contract. cleaning staking");
                        stake.Status = StakingStatus.Withdrawn;
                        _dbContext.Stakings.Update(stake);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            _logger.Information($"Finished");
            Console.ReadLine();
        }
    }
}