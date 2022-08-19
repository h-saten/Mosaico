using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectDocuments
{
    public class GetProjectDocumentsQueryResponse
    {
        public List<DocumentDTO> Documents { get; set; }
    }
}