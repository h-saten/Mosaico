using Autofac;
using Mosaico.Integration.Sms.Abstraction;

namespace Mosaico.Integration.Sms.Local
{
    public class LocalSmsSenderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<LocalSmsClient>().As<ISmsSender>();
        }
    }
}