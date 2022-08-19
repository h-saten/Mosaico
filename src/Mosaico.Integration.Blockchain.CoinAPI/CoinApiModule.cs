using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Integration.Blockchain.CoinAPI.Configurations;

namespace Mosaico.Integration.Blockchain.CoinAPI
{
    public class CoinApiModule : Module
    {
        private readonly IConfiguration _configuration;

        public CoinApiModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var config = new CoinApiConfiguration();
            _configuration.GetSection(CoinApiConfiguration.SectionName).Bind(config);
            builder.RegisterInstance(config).AsSelf();
            if (config.IsEnabled)
            {
                builder.RegisterType<CoinApiExchangeRepository>().As<IExchangeRateRepository>();
            }
            else
            {
                builder.RegisterType<FakeExchangeRateRepository>().As<IExchangeRateRepository>();
            }
        }
    }
}