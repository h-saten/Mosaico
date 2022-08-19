using Mosaico.Application.DocumentManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Queries.GetProjectDocuments
{
    public class GetProjectDocumentsQueryResponse
    {
        public IEnumerable<ProjectDocumentDTO> Documents { get; set; }
    }
}
