namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class ContractStakingConfiguration
    {
        public string StakingAddress { get; set; }
        public string StakerPrivateKey { get; set; }
        public decimal Amount { get; set; }
    }
}