using Autofac;
using Mosaico.Authorization.Base;

namespace Mosaico.API.Base
{
    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<CurrentUserContext>().As<ICurrentUserContext>().InstancePerLifetimeScope();
        }
    }
}