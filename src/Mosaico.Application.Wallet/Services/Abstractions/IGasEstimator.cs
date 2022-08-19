using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IGasEstimator
    {
        string PaymentMethod { get; }
        Task<List<DeploymentEstimateDTO>> EstimateDeploymentAsync();
        Task<List<DeploymentEstimateDTO>> EstimateTransferAsync(string ticker);
        Task<List<DeploymentEstimateDTO>> EstimateTransferAsync();
    }
}