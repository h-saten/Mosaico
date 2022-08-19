namespace Mosaico.Application.Wallet.Queries.GetPaymentDetails
{
    public class GetPaymentDetailsQueryResponse
    {
        public string Account { get; set; }
        public string BankName { get; set; }
        public string Swift { get; set; }
        public string Key { get; set; }
        public string AccountAddress { get; set; }
    }
}