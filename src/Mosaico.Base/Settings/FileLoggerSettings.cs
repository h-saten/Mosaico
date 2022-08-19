using Newtonsoft.Json;

namespace Mosaico.Base.Settings
{
    public class FileLoggerSettings : LoggerSettingBase
    {
        [JsonProperty("Path")] public string Path { get; set; }
    }
}