using System;
using MediatR;

namespace Mosaico.Application.Wallet.Queries.GetVesting
{
    public class GetVestingQuery : IRequest<GetVestingQueryResponse>
    {
        public Guid TokenId { get; set; }
    }
}