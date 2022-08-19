using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.KYC.Passbase.Abstractions;
using Mosaico.KYC.Passbase.Configurations;

namespace Mosaico.KYC.Passbase
{
    public class PassbaseModule : Module
    {
        private readonly IConfiguration _configuration;

        public PassbaseModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<PassbaseClient>().As<IPassbaseClient>();
            var config = new PassbaseConfig();
            _configuration.GetSection(PassbaseConfig.SectionName).Bind(config);
            builder.RegisterInstance(config).AsSelf();
        }
    }
}