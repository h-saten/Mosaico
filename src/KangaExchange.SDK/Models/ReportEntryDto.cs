namespace KangaExchange.SDK.Models
{
    public class ReportEntryDto
    {
        public string BuyCode { get; set; }
        public string PartnerEmail { get; set; }
        public string Currency { get; set; }
        public int Cnt { get; set; }
        public string Quantity { get; set; }
        public string Value { get; set; }
        public string PartnerFee { get; set; }
        public string PartnerBonus { get; set; }
    }
}