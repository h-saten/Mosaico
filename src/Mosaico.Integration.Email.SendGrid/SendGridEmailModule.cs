using Autofac;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.Email.SendGridEmail.Configurations;
using Mosaico.Integration.Email.SendGridEmail.Validators;
using System;

namespace Mosaico.Integration.Email.SendGridEmail
{
    public class SendGridEmailModule : Module
    {
        private readonly SendGridEmailConfig _config = new SendGridEmailConfig();

        public SendGridEmailModule(IConfiguration configuration)
        {
            configuration.GetSection(SendGridEmailConfig.SectionName).Bind(_config);
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterType<SendGridEmailClient>().As<IEmailSender>();
            builder.RegisterType<EmailValidator>().As<IValidator<Abstraction.Email>>();
        }
    }
}
