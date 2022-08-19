namespace KangaExchange.SDK.Models
{
    internal class WalletShiftRequest
    {
        public string from { get; set; }
        public string to { get; set; }
        public string currency { get; set; }
        public string quantity { get; set; }
        public string apiKey { get; set; }
    }
}