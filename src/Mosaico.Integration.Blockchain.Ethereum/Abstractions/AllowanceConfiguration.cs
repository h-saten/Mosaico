using System.Numerics;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class AllowanceConfiguration
    {
        public string ContractAddress { get; set; }
        public string OwnerPrivateKey { get; set; }
        public string SpenderAddress { get; set; }
        public decimal Amount { get; set; }
        public int Decimals { get; set; }
        public string OwnerAddress { get; set; }
    }
}