using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.Staking.GetRewardEstimate
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    [Cache("{{Id}}_{{UserId}}", ExpirationInMinutes = 2)]
    public class GetRewardEstimateQuery : IRequest<GetRewardEstimateQueryResponse>
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
    }
}