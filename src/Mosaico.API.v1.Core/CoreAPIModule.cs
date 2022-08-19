using Autofac;
using Mosaico.Application.Core;

namespace Mosaico.API.v1.Core
{
    public class CoreAPIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<CoreApplicationModule>();
        }
    }
}