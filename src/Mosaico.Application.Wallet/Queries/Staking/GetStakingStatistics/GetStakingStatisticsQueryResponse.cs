namespace Mosaico.Application.Wallet.Queries.Staking.GetStakingStatistics
{
    public class GetStakingStatisticsQueryResponse
    {
        public decimal TotalInStaking { get; set; }
        public decimal RewardClaimed { get; set; }
        public decimal ActiveStaking { get; set; }
    }
}