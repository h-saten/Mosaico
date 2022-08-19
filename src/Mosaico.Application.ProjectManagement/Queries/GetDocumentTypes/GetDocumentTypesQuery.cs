using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetDocumentTypes
{
    // [Cache]
    public class GetDocumentTypesQuery : IRequest<GetDocumentTypesQueryResponse>
    {
    }
}