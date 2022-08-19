using System;
using System.Threading.Tasks;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.Models.BuyClient;
using Newtonsoft.Json;

namespace KangaExchange.SDK.Abstractions
{
    public class KangaApiEstimateCurrencyDto
    {
        public decimal Quantity { get; set; }
        public decimal Value { get; set; }
    }

    public class KangaApiEstimatesDto
    {
        public KangaApiEstimateCurrencyDto BTC { get; set; }
        public KangaApiEstimateCurrencyDto ETH { get; set; }
        public KangaApiEstimateCurrencyDto oPLN { get; set; }
        public KangaApiEstimateCurrencyDto oEUR { get; set; }
        public KangaApiEstimateCurrencyDto oUSD { get; set; }
        public KangaApiEstimateCurrencyDto USDT { get; set; }
    }

    public class EstimatesApiResponseDto
    {
        public bool BuyCodeRequired { get; set; }
        public KangaApiEstimatesDto Estimates { get; set; }
        public string Result { get; set; }
    }
    
    public class BuyRequestData
    {
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }
        public string quantity { get; set; }
        public string paymentType { get; set; }
        public string email { get; set; }
        public string buyCode { get; set; }
        public string customRedirectUrl { get; set; }
    }
        
    public class BuyResponseDto
    {
        public string Result { get; set; }
        [JsonIgnore]
        public string OrderId { get; set; }
        public string RedirectUrl { get; set; }
    }
    
    public interface IKangaBuyApiClient
    {
        Task<EstimatesResponseDto> GetEstimatesAsync(string tokenTicker);
        Task<BuyResponseDto> BuyAsync(Action<BuyParams> configuration);
        Task<TransactionResponseDto> GetTransaction(string id);
    }
}