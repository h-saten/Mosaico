using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Integration.SignalR.Hubs.Core
{
    public class CountersHub : HubBase, ICountersDispatcher
    {
        private readonly IHubContext<CountersHub> _hubContext;

        public CountersHub(IHubContext<CountersHub> hubContext, ILogger logger = null) : base(logger)
        {
            _hubContext = hubContext;
        }

        public async Task DispatchCounterAsync(string userId, KeyValuePair<string, int> counter)
        {
            Logger?.Verbose($"Sending new counters to {userId}");
            await _hubContext.Clients.User(userId).SendAsync("updateCounters", counter);
        }
    }
}