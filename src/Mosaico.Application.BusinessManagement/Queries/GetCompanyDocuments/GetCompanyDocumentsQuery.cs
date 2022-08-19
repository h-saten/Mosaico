using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyDocuments
{
    // [Cache("{{CompanyId}}")]
    public class GetCompanyDocumentsQuery : IRequest<GetCompanyDocumentsQueryResponse>
    {
        public Guid CompanyId { get; set; }
    }
}
