namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class BuyTokensConfiguration
    {
        public string Beneficiary { get; set; }
        public decimal Amount { get; set; }
        public string SenderPrivateKey { get; set; }
    }
}