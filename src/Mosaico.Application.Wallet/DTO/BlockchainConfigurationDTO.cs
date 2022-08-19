namespace Mosaico.Application.Wallet.DTO
{
    public class BlockchainConfigurationDTO
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public bool IsDefault { get; set; }
        public string Endpoint { get; set; }
        public string ChainId { get; set; }
        public string EtherscanUrl { get; set; }
    }
}