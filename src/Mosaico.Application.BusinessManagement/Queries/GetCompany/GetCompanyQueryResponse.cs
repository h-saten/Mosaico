using Mosaico.Application.BusinessManagement.DTOs;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompany
{
    public class GetCompanyQueryResponse
    {
        public CompanyDTO Company { get; set; }
        public bool IsSubscribed { get; set; }
    }
}