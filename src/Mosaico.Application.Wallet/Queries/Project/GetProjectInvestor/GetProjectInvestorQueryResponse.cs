using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.Wallet.Queries.Project.GetProjectInvestor
{
    public class GetProjectInvestorQueryResponse
    {
        public ProjectInvestorDTO User { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalPayedInUSD { get; set; }
        public List<ProjectTransactionDTO> Transactions { get; set; }
    }
}