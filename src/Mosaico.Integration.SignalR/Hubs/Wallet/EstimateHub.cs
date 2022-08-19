using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.DTO;
using Serilog;

namespace Mosaico.Integration.SignalR.Hubs.Wallet
{
    public class EstimateHub : HubBase, IEstimateDispatcher
    {
        private readonly IHubContext<EstimateHub> _hubContext;

        public EstimateHub(IHubContext<EstimateHub> hubContext, ILogger logger = null) : base(logger)
        {
            _hubContext = hubContext;
        }

        public async Task DispatchAsync(List<DeploymentEstimateDTO> estimates)
        {
            Logger?.Verbose($"Sending new gas estimates to all");
            await _hubContext.Clients.All.SendAsync("updateGasEstimate", estimates);
        }
    }
}