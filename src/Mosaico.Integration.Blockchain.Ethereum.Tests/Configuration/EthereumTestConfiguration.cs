namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Configuration
{
    public class EthereumTestConfiguration
    {
        public const string SectionName = "EthereumTest";
        
        public string BalanceCheckAddress { get; set; }
        public string AdminWalletAddress { get; set; }
    }
}