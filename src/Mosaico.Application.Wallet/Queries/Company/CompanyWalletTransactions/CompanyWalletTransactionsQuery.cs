using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyWalletTransactions
{
    [Cache("{{CompanyId}}", ExpirationInMinutes = 3)]
    public class CompanyWalletTransactionsQuery : IRequest<CompanyWalletTransactionsResponse>
    {
        public Guid CompanyId { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 30;
    }
}