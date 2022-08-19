using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.Company.GetCompanyOwnedTokens
{
    [Cache("{{CompanyId}}")]
    public class GetCompanyOwnedTokensQuery : IRequest<List<TokenDTO>>
    {
        public Guid CompanyId { get; set; }
    }
}