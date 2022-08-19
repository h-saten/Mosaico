using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceOrderRequest
    {
        [JsonProperty("prepayId")]
        public string PrepayId { get; set; }
        
        [JsonProperty("merchantTradeNo")]
        public string MerchantTradeNo { get; set; }
    }
}