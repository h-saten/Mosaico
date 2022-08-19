using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceEnv
    {
        [JsonProperty("terminalType", NullValueHandling = NullValueHandling.Ignore)]
        public string TerminalType { get; set; } = "WEB";
    }
}