using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Configurations;

namespace Mosaico.Base
{
    public class CaptchaModule : Module
    {
        private readonly IConfiguration _configuration;

        public CaptchaModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var config = new RecaptchaConfiguration();
            _configuration.GetSection(RecaptchaConfiguration.SectionName).Bind(config);
            builder.RegisterInstance(config).AsSelf();
            builder.RegisterType<RecaptchaVerificationClient>().As<ICaptchaVerificationClient>();
        }
    }
}