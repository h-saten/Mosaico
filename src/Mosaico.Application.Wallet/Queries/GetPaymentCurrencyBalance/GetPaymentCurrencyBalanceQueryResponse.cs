namespace Mosaico.Application.Wallet.Queries.GetPaymentCurrencyBalance
{
    public class GetPaymentCurrencyBalanceQueryResponse
    {
        public decimal Balance { get; set; }
        public string Network { get; set; }
        public string PaymentCurrencyTicker { get; set; }
        public string WalletAddress { get; set; }
        public string LogoUrl { get; set; }
    }
}