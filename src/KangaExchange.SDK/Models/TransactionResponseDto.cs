namespace KangaExchange.SDK.Models
{
    public class TransactionResponseDto
    {
        public string Result { get; set; }
        public string Email { get; set; }

        public decimal Quantity { get; set; } // tokens amount
        public string Token { get; set; }
        public decimal Amount { get; set; } // currency amount
        public string BuyCode { get; set; }
        public string Currency { get; set; }

        public string Status { get; set; }
        public int Code { get; set; }
    }
}