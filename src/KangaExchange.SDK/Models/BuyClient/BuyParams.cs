using KangaExchange.SDK.ValueObjects;

namespace KangaExchange.SDK.Models.BuyClient
{
    public class BuyParams
    {
        public KangaPaymentCurrency PaymentCurrency { get; set; }
        public string TokenSymbol { get; set; }
        public decimal CurrencyAmount { get; set; }
        public string Email { get; set; }
        public string BuyCode { get; set; }
        public string CustomRedirectUrl { get; set; }
    }
}