using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Statistics.Queries.GetKPIs
{
    [Cache]
    public class GetKPIsQuery : IRequest<GetKPIsQueryResponse>
    {
        
    }
}