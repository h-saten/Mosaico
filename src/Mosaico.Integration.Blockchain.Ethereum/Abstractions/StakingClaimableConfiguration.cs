namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class StakingClaimableConfiguration
    {
        public string StakingAddress { get; set; }
        public string StakerPrivateKey { get; set; }
        public string RewardTokenAddress { get; set; }
        public string Wallet { get; set; }
    }
}