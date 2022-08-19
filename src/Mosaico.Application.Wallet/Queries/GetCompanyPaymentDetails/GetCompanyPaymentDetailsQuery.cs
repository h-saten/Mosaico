using System;
using MediatR;

namespace Mosaico.Application.Wallet.Queries.GetCompanyPaymentDetails
{
    public class GetCompanyPaymentDetailsQuery : IRequest<GetCompanyPaymentDetailsQueryResponse>
    {
        public Guid CompanyId { get; set; }
        public string Currency { get; set; }
    }
}