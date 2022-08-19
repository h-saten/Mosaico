using Autofac;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using Microsoft.Extensions.Configuration;

namespace KangaExchange.SDK
{
    public class KangaModule : Module
    {
        private readonly IConfiguration _configuration;

        public KangaModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var kangaConfiguration = new KangaConfiguration();
            _configuration.GetSection(KangaConfiguration.SectionName).Bind(kangaConfiguration);
            builder.RegisterInstance(kangaConfiguration).AsSelf();
            builder.RegisterType<KangaAuthClient>().As<IKangaAuthAPIClient>();
            builder.RegisterType<SignatureService>().As<ISignatureService>();
            builder.RegisterType<KangaIssuerApiClient>().As<IKangaIssuerApiClient>();
            builder.RegisterType<KangaEstimateProcessor>().As<IKangaEstimateProcessor>();
            builder.RegisterType<KangaUserApiClient>().As<IKangaUserApiClient>();
            builder.RegisterType<KangaBuyApiClient>().As<IKangaBuyApiClient>();
            builder.RegisterType<KangaTokenDistributionApiClient>().As<IKangaTokenDistributionApiClient>();
            builder.RegisterType<KangaMarketApiClient>().As<IKangaMarketApiClient>();
        }
    }
}