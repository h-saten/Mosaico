namespace KangaExchange.SDK.Models
{
    public class KangaPaymentCurrencyDto
    {
        public KangaPaymentCurrencyEnumDto Currency { get; set; }
        public bool IsCryptoCurrency { get; set; }
        public string DisplayTicker { get; set; }
        public string OriginalTicker { get; set; }
        public decimal CurrencyAmount { get; set; } // Quantity
        public decimal TokensAmount { get; set; } // Value
    }
}