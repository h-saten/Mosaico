using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Payments.Binance.Abstractions;
using Mosaico.Payments.Binance.Configurations;

namespace Mosaico.Payments.Binance
{
    public class BinanceModule : Module
    {
        private readonly IConfiguration _configuration;

        public BinanceModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<MosaicoBinanceClient>().As<IBinanceClient>();
            var settings = new BinanceConfiguration();
            _configuration.GetSection(BinanceConfiguration.SectionName).Bind(settings);
            builder.RegisterInstance(settings);
        }
    }
}