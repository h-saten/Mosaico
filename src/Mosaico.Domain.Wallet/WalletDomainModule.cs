using Autofac;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Repositories;

namespace Mosaico.Domain.Wallet
{
    public class WalletDomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<WalletBalanceSnapshotRepository>().As<IWalletBalanceSnapshotRepository>();
        }
    }
}