using Mosaico.Application.DocumentManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Queries.GetCompanyDocuments
{
    public class GetCompanyDocumentsQueryResponse
    {
        public IEnumerable<CompanyDocumentDTO> Documents { get; set; }
    }
}
