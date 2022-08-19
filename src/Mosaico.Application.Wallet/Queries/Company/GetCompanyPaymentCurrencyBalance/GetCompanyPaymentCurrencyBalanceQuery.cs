using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.Company.GetCompanyPaymentCurrencyBalance
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    [Cache("{{CompanyId}}", ExpirationInMinutes = 1)]
    public class GetCompanyPaymentCurrencyBalanceQuery : IRequest<GetCompanyPaymentCurrencyBalanceQueryResponse>
    {
        public Guid CompanyId { get; set; }
    }
}