namespace Mosaico.SDK.Wallet.Models
{
    public class PaymentCurrency
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string ContractAddress { get; set; }
        public bool IsNativeCurrency { get; set; }
    }
}