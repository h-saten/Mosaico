using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.DTO;
using Mosaico.Integration.SignalR.Hubs.Wallet;

namespace Mosaico.Integration.SignalR.Hubs.Project
{
    public class TokenPageHub : HubBase, ITokenPageDispatcher
    {
        private readonly IHubContext<EstimateHub> _hubContext;

        public TokenPageHub(IHubContext<EstimateHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task DispatchCoverUpdatedAsync(string coverUrl)
        {
            await _hubContext.Clients.All.SendAsync("updatePageCover", coverUrl);
        }

        public async Task DispatchLogoUpdatedAsync(string logoUrl)
        {
            await _hubContext.Clients.All.SendAsync("updatePageLogo", logoUrl);
        }

        public async Task DispatchPurchaseSuccessful(PurchaseSucceededDTO payload)
        {
            await _hubContext.Clients.All.SendAsync("updatePurchaseInfo", payload);
        }
    }
}