namespace Mosaico.Application.Wallet.Queries.Company.GetCompanyPaymentCurrencyBalance
{
    public class GetCompanyPaymentCurrencyBalanceQueryResponse
    {
        public decimal Balance { get; set; }
        public string Network { get; set; }
        public string PaymentCurrencyTicker { get; set; }
        public string WalletAddress { get; set; }
        public string LogoUrl { get; set; }
    }
}