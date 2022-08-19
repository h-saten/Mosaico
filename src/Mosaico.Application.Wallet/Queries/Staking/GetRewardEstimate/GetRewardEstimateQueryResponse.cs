using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.Staking.GetRewardEstimate
{
    public class GetRewardEstimateQueryResponse
    {
        public TokenDTO Token { get; set; }
        public decimal Balance { get; set; }
    }
}