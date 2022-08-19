using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IStakingService
    {
        Task<string> StakeAsync(string network, Action<ContractStakingConfiguration> config);
        Task<List<Models.Stake>> GetWalletStakes(string network, string stakingContract, string userWallet);
        Task<string> WithdrawAsync(string network, Action<StakingWithdrawalConfiguration> buildConfig);
        Task<string> ClaimAsync(string network, Action<StakingClaimConfiguration> buildConfig);
        Task<string> DistributeAsync(string network, Action<StakingDistributeConfiguration> buildConfig);
        Task<decimal> GetClaimableAmountAsync(string network, Action<StakingClaimableConfiguration> buildConfig);
        Task<decimal> GetBalanceOfAsync(string network, string contract, string wallet);
    }
}