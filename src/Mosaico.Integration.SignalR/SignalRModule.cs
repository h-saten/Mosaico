using Autofac;
using Microsoft.AspNetCore.SignalR;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.Hubs.Core;
using Mosaico.Integration.SignalR.Hubs.Project;
using Mosaico.Integration.SignalR.Hubs.Wallet;

namespace Mosaico.Integration.SignalR
{
    public class SignalRModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(new JwtUserIdProvider()).As<IUserIdProvider>();
            builder.RegisterType<CountersHub>().As<ICountersDispatcher>();
            builder.RegisterType<EstimateHub>().As<IEstimateDispatcher>();
            builder.RegisterType<DaoHub>().As<IDaoDispatcher>();
            builder.RegisterType<TokenPageHub>().As<ITokenPageDispatcher>();
            builder.RegisterType<WalletHub>().As<IWalletDispatcher>();
            builder.RegisterType<CrowdsaleHub>().As<ICrowdsaleDispatcher>();
        }
    }
}