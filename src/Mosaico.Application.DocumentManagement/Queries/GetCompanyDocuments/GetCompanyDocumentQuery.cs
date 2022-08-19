using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Queries.GetCompanyDocuments
{
    public class GetCompanyDocumentQuery : IRequest<GetCompanyDocumentsQueryResponse>
    {
        public Guid CompanyId { get; set; }
        public string Language { get; set; }
    }
}
