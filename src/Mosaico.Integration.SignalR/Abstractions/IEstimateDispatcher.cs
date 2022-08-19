using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Integration.SignalR.Abstractions
{
    public interface IEstimateDispatcher
    {
        Task DispatchAsync(List<DeploymentEstimateDTO> estimates);
    }
}