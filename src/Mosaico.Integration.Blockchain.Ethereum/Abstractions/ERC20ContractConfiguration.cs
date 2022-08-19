using System.Numerics;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class ERC20ContractConfiguration
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public BigInteger InitialSupply { get; set; }
        public bool IsMintable { get; set; }
        public bool IsBurnable { get; set; }
        public string Version { get; set; }
        public string OwnerAddress { get; set; }
        public string PrivateKey { get; set; }
        public bool IsGovernance { get; set; }
    }
}