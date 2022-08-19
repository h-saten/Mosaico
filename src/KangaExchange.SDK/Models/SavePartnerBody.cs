namespace KangaExchange.SDK.Models
{
    internal class SavePartnerBody
    {
        public string appId { get; set; }
        public long nonce { get; set; }
        public string ieoCode { get; set; }
        public string email { get; set; }
        public string buyCode { get; set; }
        public string feeRate { get; set; }
        public string bonusRate { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}