using System;
using Autofac;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.SDK.Wallet
{
    public class WalletSDKModule : Module
    {
        public static readonly Type MappingSDKProfileType = typeof(WalletSDKMapperProfile);
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<WalletClient>().As<IWalletClient>();
        }
    }
}