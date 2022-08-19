using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Staking.GetStakings
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetStakingsQuery : IRequest<GetStakingsQueryResponse>
    {
        public string UserId { get; set; }
    }
}