using Mosaico.Base.Settings;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Mosaico.Base.Extensions
{
    public static class ConsoleLoggerExtensions
    {
        public static LoggerConfiguration UseConsoleLogger(this LoggerConfiguration configuration,
            ConsoleLoggerSettings settings)
        {
            if (configuration != null && settings is {Enabled: true})
                configuration.WriteTo
                    .Console(settings.MinLevel, settings.Template, theme: AnsiConsoleTheme.Code)
                    .Enrich.FromLogContext();

            return configuration;
        }
    }
}