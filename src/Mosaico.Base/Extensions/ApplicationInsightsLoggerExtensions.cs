using Mosaico.Base.Settings;
using Serilog;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace Mosaico.Base.Extensions
{
    public static class ApplicationInsightsLoggerExtensions
    {
        public static LoggerConfiguration UseApplicationInsights(this LoggerConfiguration configuration,
            ApplicationInsightsLoggerSettings settings)
        {
            if (configuration != null && settings is {Enabled: true})
                configuration.WriteTo.ApplicationInsights(settings.InstrumentationKey,
                    new TraceTelemetryConverter(), settings.MinLevel)
                    .Enrich.FromLogContext();
            return configuration;
        }
    }
}