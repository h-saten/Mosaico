using Autofac;
using FluentValidation;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.Email.Local.Validators;

namespace Mosaico.Integration.Email.Local
{
    public class LocalEmailSenderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<LocalEmailClient>().As<IEmailSender>();
            builder.RegisterType<EmailValidator>().As<IValidator<Abstraction.Email>>();
        }
    }
}