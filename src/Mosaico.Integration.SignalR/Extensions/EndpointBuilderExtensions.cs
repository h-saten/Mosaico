using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Routing;
using Mosaico.Integration.SignalR.Configurations;
using Mosaico.Integration.SignalR.Hubs.Core;
using Mosaico.Integration.SignalR.Hubs.Project;
using Mosaico.Integration.SignalR.Hubs.Wallet;

namespace Mosaico.Integration.SignalR.Extensions
{
    public static class EndpointBuilderExtensions
    {
        public static void AddCoreHubs(this IEndpointRouteBuilder endpoints, SignalrConfiguration config)
        {
            endpoints.MapHub<CountersHub>("/hubs/counters", options =>
            {
                options.Transports = config.TransportType;
                options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            });
            
            endpoints.MapHub<EstimateHub>("/hubs/estimates", options =>
            {
                options.Transports = config.TransportType;
                options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            });
            
            endpoints.MapHub<DaoHub>("/hubs/companyCreation", options =>
            {
                options.Transports = config.TransportType;
                options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            });
            
            endpoints.MapHub<TokenPageHub>("/hubs/tokenPage", options =>
            {
                options.Transports = config.TransportType;
                options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            });
            
            endpoints.MapHub<WalletHub>("/hubs/wallet", options =>
            {
                options.Transports = config.TransportType;
                options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            });
            
            endpoints.MapHub<CrowdsaleHub>("/hubs/crowdsale", options =>
            {
                options.Transports = config.TransportType;
                options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            });
        }
    }
}