using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Vesting.GetPersonalVestings
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class GetPersonalVestingsQuery : IRequest<GetPersonalVestingsQueryResponse>
    {
        public Guid TokenId { get; set; }
    }
}