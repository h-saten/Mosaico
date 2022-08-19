namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class VaultSendConfiguration
    {
        public string VaultAddress { get; set; }
        public decimal Amount { get; set; }
        public string Recipient { get; set; }
        public string Id { get; set; }
    }
}