namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class StakingContractConfiguration
    {
        public decimal Reward { get; set; }
        public long RewardPeriodInDays { get; set; }
        public string ERC20Address { get; set; }
        public string PrivateKey { get; set; }
    }
}