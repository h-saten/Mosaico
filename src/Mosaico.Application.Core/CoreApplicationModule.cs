using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Mosaico.Application.Core
{
    public class CoreApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
        }
    }
}