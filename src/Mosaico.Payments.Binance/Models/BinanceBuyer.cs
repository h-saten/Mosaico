using Newtonsoft.Json;

namespace Mosaico.Payments.Binance.Models
{
    public class BinanceBuyerName
    {
        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
    }
    
    public class BinanceBuyer
    {
        [JsonProperty("referenceBuyerId", NullValueHandling = NullValueHandling.Ignore)]
        public string ReferenceBuyerId { get; set; }
        [JsonProperty("buyerName", NullValueHandling = NullValueHandling.Ignore)]
        public BinanceBuyerName BuyerName { get; set; }
    }
}