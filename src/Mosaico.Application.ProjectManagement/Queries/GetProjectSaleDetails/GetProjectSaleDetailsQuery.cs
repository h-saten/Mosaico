using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectSaleDetails
{
    // [Cache("{{UniqueIdentifier}}")]
    public class GetProjectSaleDetailsQuery : IRequest<GetProjectSaleDetailsResponse>
    {
        public string UniqueIdentifier { get; set; }
    }
}