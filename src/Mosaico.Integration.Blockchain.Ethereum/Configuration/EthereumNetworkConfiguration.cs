namespace Mosaico.Integration.Blockchain.Ethereum.Configuration
{
    public class EthereumNetworkConfiguration
    {
        public string Name { get; set; }
        public string Endpoint { get; set; }
        public string Chain { get; set; }
        public string AdminAccountProviderType { get; set; }
        public EthereumAdminAccount AdminAccount { get; set; }
        public string EtherscanApiToken { get; set; }
        public string EtherscanApiUrl { get; set; }
        public bool IsDefault { get; set; }
        public string LogoUrl { get; set; }
        public string EtherscanUrl { get; set; }
        public double BlockTime { get; set; }
    }
}