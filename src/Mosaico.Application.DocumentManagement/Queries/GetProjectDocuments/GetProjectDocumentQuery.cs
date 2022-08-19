using MediatR;
using System;

namespace Mosaico.Application.DocumentManagement.Queries.GetProjectDocuments
{
    public class GetProjectDocumentQuery : IRequest<GetProjectDocumentsQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
    }
}
