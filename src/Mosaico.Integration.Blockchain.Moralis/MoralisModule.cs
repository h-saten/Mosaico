using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Integration.Blockchain.Moralis.ApiClients;
using Mosaico.Integration.Blockchain.Moralis.Configuration;

namespace Mosaico.Integration.Blockchain.Moralis
{
    public class MoralisModule : Module
    {
        private readonly IConfiguration _configuration;

        public MoralisModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var config = new MoralisConfiguration();
            _configuration.GetSection(MoralisConfiguration.SectionName).Bind(config);
            builder.RegisterInstance(config);
            builder.RegisterType<MoralisAccountApiFakeClient>().As<IAccountRepository>();
            // builder.RegisterType<MoralisTokenApiClient>().As<ITokenRepository>();
            // builder.RegisterType<MoralisTokensApiFakeClient>().As<ITokenRepository>();
        }
    }
}