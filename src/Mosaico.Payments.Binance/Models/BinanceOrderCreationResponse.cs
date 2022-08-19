using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceOrderCreationResponse
    {
        [JsonProperty("prepayId")]
        public string PrepayId { get; set; }
        [JsonProperty("terminalType")]
        public string TerminalType { get; set; }
        [JsonProperty("expireTime")]
        public long ExpireTime { get; set; }
        [JsonProperty("qrcodeLink")]
        public string QrCodeLink { get; set; }
        [JsonProperty("qrContent")]
        public string QrContent { get; set; }
        [JsonProperty("checkoutUrl")]
        public string CheckoutUrl { get; set; }
        [JsonProperty("deeplink")]
        public string DeepLink { get; set; }
        [JsonProperty("universalUrl")]
        public string UniversalUrl { get; set; }
    }
}