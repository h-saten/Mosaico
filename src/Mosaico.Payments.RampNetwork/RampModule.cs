using Autofac;
using Mosaico.Payments.RampNetwork.Abstractions;

namespace Mosaico.Payments.RampNetwork
{
    public class RampModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<RampClient>().As<IRampClient>();
        }
    }
}