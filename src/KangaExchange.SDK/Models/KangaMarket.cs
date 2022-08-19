namespace KangaExchange.SDK.Models
{
    public class KangaMarket
    {
        public string Id { get; set; }
        public string LastPrice { get; set; }
        public string Change { get; set; }
        public string Volume { get; set; }
        public string BuyingCurrency { get; set; }
        public string PayingCurrency { get; set; }
        public string Type { get; set; }
        public string MinAmount { get; set; }
        public bool MainMarket { get; set; }
        public int PricePrecision { get; set; }
        public int Index { get; set; }
        public string Opening { get; set; }
        public string BidsAvailableSince { get; set; }
        public string AsksAvailableSince { get; set; }
    }
}