using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProject
{
    // [Cache("{{UniqueIdentifier}}")]
    public class GetProjectQuery : IRequest<GetProjectQueryResponse>
    {
        public string UniqueIdentifier { get; set; }
    }
}