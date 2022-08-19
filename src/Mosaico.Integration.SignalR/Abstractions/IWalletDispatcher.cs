using System;
using System.Threading.Tasks;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Integration.SignalR.Abstractions
{
    public interface IWalletDispatcher
    {
        Task DispatchTokenDeployed(string userId, TokenCreatedDTO payload);
        Task DispatchTokenDeploymentFailed(string userId, string error);
        Task TokenMinted(string userId, Guid tokenId);
        Task TokenMintingFailed(string userId, string error);
        Task TokenBurned(string userId, Guid tokenId);
        Task TokenBurningFailed(string userId, string error);
        Task SentCurrency(string userId, Guid transactionId);
        Task SentCurrencyFailed(string userId, string error);
        Task AirdropDispatchFailed(string userId, string error);
        Task AirdropDispatched(string userId, string transactionHash);
        Task StakingDeployed(string userId, string contractAddress);
        Task StakingDeploymentFailed(string userId, string error);
        Task Staked(string userId, string transactionHash);
        Task StakeFailed(string userId, string error);
        Task StakeWithdrawalSucceeded(string userId, string transactionHash);
        Task StakeWithdrawalFailed(string userId, string error);
        Task PartnerRewardClaimed(string userId, string transactionHash);
        Task PartnerRewardFailed(string userId, string transactionHash);
        Task StakeRewardClaimed(string userId, string transactionHash);
        Task StakeRewardClaimFailed(string userId, string error);
        Task StakeDistributed(string userId, string transactionHash);
        Task StakeDistributionFailed(string userId, string error);
    }
}