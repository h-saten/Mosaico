using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Integration.Sms.Abstraction;
using Mosaico.Integration.Sms.SmsLabs.Configurations;

namespace Mosaico.Integration.Sms.SmsLabs
{
    /*
     * Module which contains registrations of SMS API - SmsLabs integration
     */
    public class SmsLabsModule : Module
    {
        private readonly SmsLabsConfig _config = new();

        public SmsLabsModule(IConfiguration configuration)
        {
            configuration.GetSection(SmsLabsConfig.SectionName).Bind(_config);
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterType<SmsLabsClient>().As<ISmsSender>();
        }
    }
}