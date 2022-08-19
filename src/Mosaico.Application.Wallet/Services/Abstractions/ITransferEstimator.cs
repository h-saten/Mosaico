using System.Threading.Tasks;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface ITransferEstimator
    {
        Task<DeploymentEstimateDTO> EstimateAsync();
    }
}