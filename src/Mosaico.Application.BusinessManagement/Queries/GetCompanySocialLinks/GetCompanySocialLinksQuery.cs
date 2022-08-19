using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanySocialLinks
{
    public class GetCompanySocialLinksQuery : IRequest<GetCompanySocialLinksQueryResponse>
    {
        public Guid CompanyId { get; set; }
    }
}
