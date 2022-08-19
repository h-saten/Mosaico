using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Staking.GetStakingStatistics
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetStakingStatisticsQuery : IRequest<GetStakingStatisticsQueryResponse>
    {
        public string UserId { get; set; }
    }
}