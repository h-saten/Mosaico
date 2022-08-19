namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public class StakingContractConfiguration
    {
        public string ERC20Address { get; set; }
        public long Reward { get; set; }
        public int RewardPeriodInDays { get; set; }
    }
}