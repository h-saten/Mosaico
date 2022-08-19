using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompany
{
    // [Cache("{{UniqueIdentifier}}")]
    public class GetCompanyQuery : IRequest<GetCompanyQueryResponse>
    {
        public string UniqueIdentifier { get; set; }
    }
}