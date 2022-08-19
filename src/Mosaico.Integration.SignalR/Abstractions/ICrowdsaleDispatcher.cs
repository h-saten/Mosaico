using System;
using System.Threading.Tasks;

namespace Mosaico.Integration.SignalR.Abstractions
{
    public interface ICrowdsaleDispatcher
    {
        Task CrowdsaleDeployed(string userId);
        Task CrowdsaleDeploymentFailed(string userId, string error);
        Task StageDeployed(string userId, Guid stageId);
        Task StageDeploymentFailed(string userId, Guid stageId, string error);
        Task PurchaseSuccessful(string userId, string transactionHash);
        Task PurchaseFailed(string userId, string error);
    }
}