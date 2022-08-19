namespace Mosaico.Application.Wallet.DTO
{
    public class ExchangeRateDTO
    {
        public string BaseCurrency { get; set; }
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}