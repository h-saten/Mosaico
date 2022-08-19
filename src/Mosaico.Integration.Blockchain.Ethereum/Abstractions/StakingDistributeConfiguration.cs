namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class StakingDistributeConfiguration
    {
        public string StakingAddress { get; set; }
        public string StakerPrivateKey { get; set; }
        public string RewardTokenAddress { get; set; }
        public decimal Amount { get; set; }
        public int Decimals { get; set; } = 18;
    }
}