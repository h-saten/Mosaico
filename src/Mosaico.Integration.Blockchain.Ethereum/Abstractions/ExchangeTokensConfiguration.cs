namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class ExchangeTokensConfiguration
    {
        public string SenderPrivateKey { get; set; }
        public string PaymentTokenAddress { get; set; }
        public int PaymentTokenDecimalPlaces { get; set; }
        public string Beneficiary { get; set; }
        public decimal Amount { get; set; }
    }
}