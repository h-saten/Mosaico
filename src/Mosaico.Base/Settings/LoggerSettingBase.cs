using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog.Events;

namespace Mosaico.Base.Settings
{
    public abstract class LoggerSettingBase
    {
        [JsonProperty("Enabled")] public bool Enabled { get; set; }

        [JsonProperty("MinLevel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LogEventLevel MinLevel { get; set; }
    }
}