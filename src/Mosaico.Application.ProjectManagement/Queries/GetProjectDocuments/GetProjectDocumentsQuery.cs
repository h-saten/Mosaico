using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectDocuments
{
    // [Cache("{{ProjectId}}_{{Language}}", ExpirationInMinutes = 1)]
    public class GetProjectDocumentsQuery : IRequest<GetProjectDocumentsQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
    }
}