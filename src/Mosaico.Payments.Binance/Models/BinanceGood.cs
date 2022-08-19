using System;
using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceGood
    {
        public static BinanceGood CreateDigital(Guid id, string name)
        {
            return new BinanceGood
            {
                GoodsType = "02",
                GoodsCategory = "Z000",
                ReferenceGoodsId = id.ToString(),
                GoodsName = name
            };
        }
        [JsonProperty("goodsType", NullValueHandling = NullValueHandling.Ignore)]
        public string GoodsType { get; set; }
        [JsonProperty("goodsCategory", NullValueHandling = NullValueHandling.Ignore)]
        public string GoodsCategory { get; set; }
        [JsonProperty("referenceGoodsId", NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceGoodsId { get; set; }
        [JsonProperty("goodsName", NullValueHandling = NullValueHandling.Ignore)]
        public string GoodsName { get; set; }
        [JsonProperty("goodsDetail", NullValueHandling = NullValueHandling.Ignore)]
        public string GoodsDetail { get; set; }
    }
}