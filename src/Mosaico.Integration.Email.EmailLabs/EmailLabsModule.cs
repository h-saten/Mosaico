using Autofac;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.Email.EmailLabs.Configurations;
using Mosaico.Integration.Email.EmailLabs.Validators;

namespace Mosaico.Integration.Email.EmailLabs
{
    /*
     * Module which contains registrations of Email API - EmailLabs integration
     */
    public class EmailLabsModule : Module
    {
        private readonly EmailLabsConfig _config = new EmailLabsConfig();

        public EmailLabsModule(IConfiguration configuration)
        {
            configuration.GetSection(EmailLabsConfig.SectionName).Bind(_config);
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterType<EmailLabsClient>().As<IEmailSender>();
            builder.RegisterType<EmailValidator>().As<IValidator<Abstraction.Email>>();
        }
    }
}