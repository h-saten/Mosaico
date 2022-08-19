using Autofac;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Policies;
using Mosaico.Domain.Identity.Repositories;

namespace Mosaico.Domain.Identity
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<UserReadRepository>().As<IUserReadRepository>();
            builder.RegisterType<UserWriteRepository>().As<IUserWriteRepository>();
            builder.RegisterType<PhoneNumberConfirmationCodeGenerationPolicy>().As<IPhoneNumberConfirmationCodeGenerationPolicy>();
            builder.RegisterType<SecurityCodeRepository>().As<ISecurityCodeRepository>();
            builder.RegisterType<PhoneNumberConfirmationCodesRepository>().As<IPhoneNumberConfirmationCodesRepository>();
        }
    }
}