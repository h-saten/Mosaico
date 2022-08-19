using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetTokenDistribution
{
    public class GetTokenDistributionQuery : IRequest<List<TokenDistributionDTO>>
    {
        public Guid TokenId { get; set; }
    }
}