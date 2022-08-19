using System;
using System.Collections.Generic;
using Mosaico.Application.BusinessManagement.DTOs;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyDocuments
{
    public class GetCompanyDocumentsQueryResponse
    {
        public List<CompanyDocumentDTO> Documents { get; set; }
    }
}
