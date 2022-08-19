using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.DTO;
using Serilog;

namespace Mosaico.Integration.SignalR.Hubs.Wallet
{
    public class DaoHub : HubBase, IDaoDispatcher
    {
        private readonly IHubContext<DaoHub> _hubContext;

        public DaoHub(IHubContext<DaoHub> hubContext, ILogger logger = null) : base(logger)
        {
            _hubContext = hubContext;
        }
        
        public async Task DispatchSucceededAsync(string userId, CompanyCreatedDTO created)
        {
            Logger?.Verbose($"Sending company created to {userId}");
            await _hubContext.Clients.User(userId).SendAsync("successfullyCreated", created);
        }
        
        public async Task DispatchFailedAsync(string userId, string error)
        {
            Logger?.Verbose($"Sending company failed to create to {userId}");
            await _hubContext.Clients.User(userId).SendAsync("failedToCreate", error);
        }

        public async Task ProposalCreated(string userId, string proposalId)
        {
            await _hubContext.Clients.User(userId).SendAsync("proposalCreated", proposalId);
        }

        public async Task ProposalCreationFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("proposalCreationFailed", error);
        }

        public async Task VoteSubmitted(string userId, string transaction)
        {
            await _hubContext.Clients.User(userId).SendAsync("voteSubmitted", transaction);
        }

        public async Task VoteFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("voteFailed", error);
        }

        public async Task VaultDeployed(string userId)
        {
            await _hubContext.Clients.User(userId).SendAsync("vaultDeployed");
        }
        
        public async Task VaultDeploymentFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("vaultDeploymentFailed", error);
        }

        public async Task DepositCreated(string userId)
        {
            await _hubContext.Clients.User(userId).SendAsync("depositCreated");
        }

        public async Task DepositCreationFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("depositCreationFailed", error);
        }
        
        public async Task VaultSent(string userId)
        {
            await _hubContext.Clients.User(userId).SendAsync("vaultSent");
        }

        public async Task VaultSendFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("vaultSendFailed", error);
        }
        
        public async Task VestingDeployed(string userId)
        {
            await _hubContext.Clients.User(userId).SendAsync("vestingDeployed");
        }

        public async Task VestingDeploymentFailed(string userId, string error)
        {
            await _hubContext.Clients.User(userId).SendAsync("vestingDeploymentFailed", error);
        }
    }
}