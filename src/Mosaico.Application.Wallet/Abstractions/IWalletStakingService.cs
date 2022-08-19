using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Application.Wallet.Abstractions
{
    public interface IWalletStakingService
    {
        Task<bool> CanStakeAsync(Staking stake);
        Task StartDeploymentAsync(Staking stake);
        Task<string> StakeAsync(Staking stake);
        Task<string> ApproveAsync(Staking stake);
        Task SetDeploymentFailed(Staking stake, string error);
        Task SetDeploymentCompleted(Staking stake);
        Task<string> ClaimAsync(Staking stake);
        Task<string> WithdrawAsync(Staking stake);
        Task<string> DistributeAsync(StakingPair pair, decimal amount, string ownerPrivateKey);
        Task<decimal> GetEstimatedRewardAsync(Staking stake);
        Task<decimal> GetStakingBalanceAsync(Staking stake);
        Task<decimal> GetStakingBalanceAsync(StakingPair pair, string userWallet);
        Task<string> ApproveWithdrawableTokenAsync(Staking stake, decimal balance);
        Task<bool> CanClaimAsync(Staking stake);
        Task<bool> RequiresApprovalAsync(Staking stake);
    }
}