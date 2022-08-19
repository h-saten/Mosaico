using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mosaico.Integration.SignalR.Abstractions;

namespace Mosaico.Integration.SignalR.Hubs.Project
{
    public class CrowdsaleHub: HubBase, ICrowdsaleDispatcher
    {
        private readonly IHubContext<CrowdsaleHub> _hubContext;

        public CrowdsaleHub(IHubContext<CrowdsaleHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task CrowdsaleDeployed(string userId)
        {
            await _hubContext.Clients.User(userId).SendAsync("crowdsaleCreated", true);
        }
        
        public async Task CrowdsaleDeploymentFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("crowdsaleDeploymentFailed", true);
        }

        public async Task StageDeployed(string userId, Guid stageId)
        {
            await _hubContext.Clients.User(userId).SendAsync("stageDeployed", stageId);
        }

        public async Task StageDeploymentFailed(string userId, Guid stageId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("stageDeploymentFailed", new { stageId, error});
        }
        
        public async Task PurchaseSuccessful(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("purchaseSuccessful", transactionHash);
        }
        
        public async Task PurchaseFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("purchaseFailed", error);
        }

        public async Task WithdrawalSucceeded(string userId, string transactionHash)
        {
            await _hubContext.Clients.User(userId).SendAsync("withdrawalSucceeded", transactionHash);
        }
        
        public async Task WithdrawalFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("withdrawalFailed", error);
        }
    }
}