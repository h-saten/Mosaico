using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.DTO;
using Serilog;

namespace Mosaico.Integration.SignalR.Hubs.Wallet
{
    public class WalletHub : HubBase, IWalletDispatcher
    {
        private readonly IHubContext<WalletHub> _hubContext;

        public WalletHub(IHubContext<WalletHub> hubContext, ILogger logger = null) : base(logger)
        {
            _hubContext = hubContext;
        }

        public async Task DispatchTokenDeployed(string userId, TokenCreatedDTO payload)
        {
            await _hubContext.Clients.User(userId).SendAsync("tokenCreated", payload);
        }

        public async Task DispatchTokenDeploymentFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("tokenCreationFailed", error);
        }

        public async Task TokenMinted(string userId, Guid tokenId)
        {
            await _hubContext.Clients.User(userId).SendAsync("tokenMinted", tokenId);
        }

        public async Task TokenMintingFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("tokenMintingFailed", error);
        }

        public async Task TokenBurned(string userId, Guid tokenId)
        {
            await _hubContext.Clients.User(userId).SendAsync("tokenBurned", tokenId);
        }

        public async Task TokenBurningFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("tokenBurningFailed", error);
        }

        public async Task SentCurrency(string userId, Guid transactionId)
        {
            await _hubContext.Clients.User(userId).SendAsync("sentCurrencySucceeded", transactionId);
        }
        
        public async Task SentCurrencyFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("sentCurrencyFailed", error);
        }

        public async Task AirdropDispatched(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("airdropDispatched", transactionHash);
        }
        
        public async Task AirdropDispatchFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("airdropDispatchFailed", error);
        }
        
        public async Task PartnerRewardClaimed(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("partnerRewardClaimed", transactionHash);
        }
        
        public async Task PartnerRewardFailed(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("partnerRewardFailed", transactionHash);
        }

        public async Task StakingDeployed(string userId, string contractAddress)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakingDeployed", contractAddress);
        }

        public async Task StakingDeploymentFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakingDeploymentFailed", error);
        }

        public async Task Staked(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeSucceeded", transactionHash);
        }

        public async Task StakeFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeFailed", error);
        }

        public async Task StakeWithdrawalSucceeded(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeWithdrawalSucceeded", transactionHash);
        }

        public async Task StakeWithdrawalFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeWithdrawalFailed", error);
        }
        
        public async Task StakeRewardClaimed(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeRewardClaimedSuccessfully", transactionHash);
        }

        public async Task StakeRewardClaimFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeRewardClaimFailed", error);
        }

        public async Task StakeDistributed(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeDistributed", transactionHash);
        }

        public async Task StakeDistributionFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("stakeDistributionFailed", error);
        }
    }
}