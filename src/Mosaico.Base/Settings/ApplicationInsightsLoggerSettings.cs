using Newtonsoft.Json;

namespace Mosaico.Base.Settings
{
    public class ApplicationInsightsLoggerSettings : LoggerSettingBase
    {
        [JsonProperty("InstrumentationKey")] public string InstrumentationKey { get; set; }
    }
}