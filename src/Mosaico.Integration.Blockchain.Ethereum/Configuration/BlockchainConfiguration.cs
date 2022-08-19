using System.Collections.Generic;

namespace Mosaico.Integration.Blockchain.Ethereum.Configuration
{
    public class BlockchainConfiguration
    {
        public const string SectionName = "Blockchain";
        public List<EthereumNetworkConfiguration> Networks { get; set; } = new List<EthereumNetworkConfiguration>();
    }
}