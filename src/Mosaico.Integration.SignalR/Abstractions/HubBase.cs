using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Mosaico.Integration.SignalR.Abstractions
{
    public abstract class HubBase : Hub
    {
        protected readonly ILogger Logger;

        protected HubBase(ILogger logger = null)
        {
            Logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            Logger?.Verbose($"{Context.UserIdentifier} connected to the signalr hub {GetType().Name}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Logger?.Verbose($"{Context.UserIdentifier} disconnected from the signalr hub {GetType().Name}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}