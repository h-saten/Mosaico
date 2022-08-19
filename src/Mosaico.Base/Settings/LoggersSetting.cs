using Newtonsoft.Json;

namespace Mosaico.Base.Settings
{
    public class LoggersSetting
    {
        [JsonIgnore] public const string SectionName = "Loggers";

        [JsonProperty("FileLogger")] public FileLoggerSettings FileLogger { get; set; }

        [JsonProperty("ConsoleLogger")] public ConsoleLoggerSettings ConsoleLogger { get; set; }

        [JsonProperty("ApplicationInsightsLogger")]
        public ApplicationInsightsLoggerSettings ApplicationInsightsLogger { get; set; }
    }
}