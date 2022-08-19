using System.Collections.Generic;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class CrowdsaleContractConfiguration
    {
        public bool IsVestingEnabled { get; set; }
        public string OwnerAddress { get; set; }
        public long StageCount { get; set; }
        public string ERC20Address { get; set; }
        public string ContractAddress { get; set; }
        public decimal SoftCapDenominator { get; set; }
        public List<string> SupportedStableCoins { get; set; }
        public string PrivateKey { get; set; }
    }
}