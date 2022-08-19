using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyWalletTokens
{
    [Cache("{{CompanyId}}_{{TokenTicker}}", ExpirationInMinutes = 3)]
    public class CompanyWalletTokensQuery : IRequest<CompanyWalletTokensQueryResponse>
    {
        public Guid CompanyId { get; set; }
        public string TokenTicker { get; set; }
    }
}