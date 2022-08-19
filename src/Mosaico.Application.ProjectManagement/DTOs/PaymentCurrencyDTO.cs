namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class PaymentCurrencyDTO
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string ContractAddress { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsNativeCurrency { get; set; }
    }
}