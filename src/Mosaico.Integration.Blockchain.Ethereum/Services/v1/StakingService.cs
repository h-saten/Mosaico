using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable;
using Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable.ContractDefinition;
using Nethereum.Web3;

namespace Mosaico.Integration.Blockchain.Ethereum.Services.v1
{
    public class StakingService : IStakingService
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;
        
        public StakingService(IEthereumClientFactory ethereumClientFactory)
        {
            _ethereumClientFactory = ethereumClientFactory;
        }

        public async Task<List<Models.Stake>> GetWalletStakes(string network, string stakingContract, string userWallet)
        {
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(GetAdminPrivateKey(network));
            var web3 = client.GetClient(account);
            var stakingService = new StakingUpgradableService(web3, stakingContract);
            var stakes = await stakingService.StakesQueryAsync(userWallet);
            if (stakes?.ReturnValue1 == null || stakes?.ReturnValue1?.Any() == false)
            {
                return new List<Models.Stake>();
            }

            return stakes.ReturnValue1.Select(rv => new Models.Stake
            {
                Active = rv.Active,
                Since = DateTimeOffset.FromUnixTimeSeconds(long.Parse(rv.Since.ToString())),
                Amount = Web3.Convert.FromWei(rv.Amount),
                Staker = rv.Staker,
                Token = rv.Token
            }).ToList();
        }
        
        public async Task<string> ClaimAsync(string network, Action<StakingClaimConfiguration> buildConfig)
        {
            var config = new StakingClaimConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.StakerPrivateKey);
            var web3 = client.GetClient(account);
            var stakingService = new StakingUpgradableService(web3, config.StakingAddress);
            return await stakingService.ClaimRequestAsync(config.TokenAddress);
        }
        
        public async Task<string> DistributeAsync(string network, Action<StakingDistributeConfiguration> buildConfig)
        {
            var config = new StakingDistributeConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.StakerPrivateKey);
            var web3 = client.GetClient(account);
            var stakingService = new StakingUpgradableService(web3, config.StakingAddress);
            var tokensToDistribute = Web3.Convert.ToWei(config.Amount, config.Decimals);
            return await stakingService.DistributeRequestAsync(config.RewardTokenAddress, tokensToDistribute);
        }
        
        public async Task<decimal> GetClaimableAmountAsync(string network, Action<StakingClaimableConfiguration> buildConfig)
        {
            var config = new StakingClaimableConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.StakerPrivateKey);
            var web3 = client.GetClient(account);
            var stakingService = new StakingUpgradableService(web3, config.StakingAddress);
            var response = await stakingService.ClaimableBalanceOfQueryAsync(config.RewardTokenAddress, config.Wallet);
            return Web3.Convert.FromWei(response, 18);
        }
        
        public async Task<decimal> GetBalanceOfAsync(string network, string contract, string wallet)
        {
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAdminAccountAsync();
            var web3 = client.GetClient(account);
            var stakingService = new StakingUpgradableService(web3, contract);
            var response = await stakingService.BalanceOfQueryAsync(wallet);
            return Web3.Convert.FromWei(response, 18);
        }

        public async Task<string> WithdrawAsync(string network, Action<StakingWithdrawalConfiguration> buildConfig)
        {
            var config = new StakingWithdrawalConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.StakerPrivateKey);
            var web3 = client.GetClient(account);
            var stakingService = new StakingUpgradableService(web3, config.StakingAddress);
            return await stakingService.WithdrawRequestAsync();
        }

        public async Task<string> StakeAsync(string network, Action<ContractStakingConfiguration> buildConfig)
        {
            var config = new ContractStakingConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(config.StakerPrivateKey);
            var web3 = client.GetClient(account);
            var tokensToStake = Web3.Convert.ToWei(config.Amount, 18);
            var stakingService = new StakingUpgradableService(web3, config.StakingAddress);
            var stakeFunction = new StakeFunction
            {
                FromAddress = account.Address,
                Amount = tokensToStake
            };
            var transactionHash = await stakingService.StakeRequestAsync(stakeFunction);
            return transactionHash;
        }

        private string GetAdminPrivateKey(string network)
        {
            var config = _ethereumClientFactory.GetConfiguration(network);
            return config.AdminAccount.PrivateKey;
        }
    }
}