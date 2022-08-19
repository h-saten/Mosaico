using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceOrderResponse
    {
        [JsonProperty("merchantId")] 
        public long MerchantId { get; set; }

        [JsonProperty("prepayId")] 
        public string PrepayId { get; set; }

        [JsonProperty("transactionId")] 
        public string TransactionId { get; set; }

        [JsonProperty("merchantTradeNo")] 
        public string MerchantTradeNo { get; set; }
        
        [JsonProperty("status")] 
        public string Status { get; set; }
        
        [JsonProperty("currency")] 
        public string Currency { get; set; }
        
        [JsonProperty("orderAmount")] 
        public string OrderAmount { get; set; }
        
        [JsonProperty("openUserId")] 
        public string OpenUserId { get; set; }
        
        [JsonProperty("transactTime")] 
        public long TransactTime { get; set; }
        
        [JsonProperty("createTime")] 
        public long CreateTime { get; set; }
    }
}