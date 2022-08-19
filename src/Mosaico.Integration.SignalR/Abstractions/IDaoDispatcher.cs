using System;
using System.Threading.Tasks;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Integration.SignalR.Abstractions
{
    public interface IDaoDispatcher
    {
        Task DispatchSucceededAsync(string userId, CompanyCreatedDTO created);
        Task DispatchFailedAsync(string userId, string error);
        Task ProposalCreated(string userId, string proposalId);
        Task ProposalCreationFailed(string userId, string error);
        Task VoteSubmitted(string userId, string transaction);
        Task VoteFailed(string userId, string error);
        Task VaultDeployed(string userId);
        Task VaultDeploymentFailed(string userId, string error);
        Task DepositCreated(string userId);
        Task DepositCreationFailed(string userId, string error);
        Task VaultSent(string userId);
        Task VaultSendFailed(string userId, string error);
        Task VestingDeployed(string userId);
        Task VestingDeploymentFailed(string userId, string error);
    }
}