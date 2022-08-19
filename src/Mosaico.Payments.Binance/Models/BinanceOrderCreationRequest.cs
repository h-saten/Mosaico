using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceOrderCreationRequest
    {
        [JsonProperty("merchantTradeNo", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchantTradeNo { get; set; }

        [JsonProperty("orderAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OrderAmount { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("goods", NullValueHandling = NullValueHandling.Ignore)]
        public BinanceGood Goods { get; set; }

        [JsonProperty("env", NullValueHandling = NullValueHandling.Ignore)]
        public BinanceEnv Env { get; set; } = new BinanceEnv();

        [JsonProperty("returnUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ReturnUrl { get; set; }

        [JsonProperty("cancelUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string CancelUrl { get; set; }

        [JsonProperty("buyer", NullValueHandling = NullValueHandling.Ignore)]
        public BinanceBuyer Buyer { get; set; }
    }
}