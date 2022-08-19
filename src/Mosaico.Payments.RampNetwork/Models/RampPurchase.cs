using System;
using Newtonsoft.Json;

namespace Mosaico.Payments.RampNetwork.Models
{
    public class RampPurchase
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
        
        [JsonProperty("finalTxHash")]
        public string FinalTxHash { get; set; }
        
        [JsonProperty("appliedFee")]
        public decimal Fee { get; set; }
        
        [JsonProperty("receiverAddress")]
        public string ReceiverAddress { get; set; }
        
        [JsonProperty("cryptoAmount")]
        public string CryptoAmount { get; set; }
        
        [JsonProperty("asset")]
        public RampAsset Asset { get; set; }
        
        [JsonProperty("fiatCurrency")]
        public string FiatCurrency { get; set; }
        
        [JsonProperty("fiatValue")]
        public decimal FiatValue { get; set; }
        
        [JsonProperty("paymentMethodType")]
        public string PaymentMethodType { get; set; }
    }
}