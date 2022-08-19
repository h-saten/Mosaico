using Autofac;
using Mosaico.Analytics.Base;

namespace Mosaico.Statistics.Local
{
    /*
     * Module which contains registrations of GA API - Google Analytics integration
     */
    public class LocalAnalyticsModule : Module
    {
        public LocalAnalyticsModule()
        {
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<LocalAnalyticsProvider>().As<ITrafficProvider>();
        }
    }
}