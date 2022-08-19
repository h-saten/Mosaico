using Newtonsoft.Json;

namespace Mosaico.Base.Settings
{
    public class ConsoleLoggerSettings : LoggerSettingBase
    {
        [JsonProperty("Template")] public string Template { get; set; }
    }
}