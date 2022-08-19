namespace Mosaico.Payments.Transak.Models
{
    public class OrderDetails
    {
        public string Id { get; set; }
        public string WalletAddress { get; set; }
        public string Status { get; set; }
        public string FiatCurrency { get; set; }
        public string CryptoCurrency { get; set; }
        public decimal FiatAmount { get; set; }
        public string Network { get; set; }
        public decimal CryptoAmount { get; set; }
        public decimal TotalFeeInFiat { get; set; }
        public decimal AmountPaid { get; set; }
        public long ReferenceCode { get; set; }
        public string TransactionHash { get; set; }
        public decimal FiatAmountInUsd { get; set; }
        public string PaymentOptionId { get; set; }
    }
}