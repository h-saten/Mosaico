using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.SDK.Relay.Abstractions;
using Mosaico.SDK.Relay.Configurations;

namespace Mosaico.SDK.Relay
{
    public class RelayModule : Module
    {
        private readonly IConfiguration _configuration;

        public RelayModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var config = new RelayConfig();
            _configuration.GetSection(RelayConfig.SectionName).Bind(config);
            builder.RegisterInstance(config);

            builder.RegisterType<MosaicoRelayClient>().As<IMosaicoRelayClient>();
        }
    }
}