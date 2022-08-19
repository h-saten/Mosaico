using MediatR;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompaniesPublicly
{
    public class GetCompaniesPubliclyQuery : IRequest<GetCompaniesPubliclyQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Search { get; set; }
    }
}