using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;

namespace Mosaico.Application.ProjectManagement.Queries.GetInvestmentPackages
{
    public class GetInvestmentPackagesQueryResponse
    {
        public List<InvestmentPackageDTO> Packages { get; set; }
    }
}