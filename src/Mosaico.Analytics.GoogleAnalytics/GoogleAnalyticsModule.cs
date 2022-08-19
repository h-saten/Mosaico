using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Analytics.Base;
using Mosaico.Secrets.KeyVault;
using Mosaico.Statistics.GoogleAnalytics.Abstractions;
using Mosaico.Statistics.GoogleAnalytics.Configuration;

namespace Mosaico.Statistics.GoogleAnalytics
{
    /*
     * Module which contains registrations of GA API - Google Analytics integration
     */
    public class GoogleAnalyticsModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly GoogleAnalyticsConfiguration _config = new();

        public GoogleAnalyticsModule(IConfiguration configuration)
        {
            _configuration = configuration;
            configuration.GetSection(GoogleAnalyticsConfiguration.SectionName).Bind(_config);
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterModule(new KeyVaultModule(_configuration));
            builder.RegisterType<GoogleAnalyticsProjectStatistics>().As<ITrafficProvider>();
            builder.RegisterType<GoogleAnalyticsConnector>().As<IAnalyticsReportingConnector>();
        }
    }
}