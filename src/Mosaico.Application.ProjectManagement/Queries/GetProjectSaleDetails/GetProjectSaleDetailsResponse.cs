using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectSaleDetails
{
    public class GetProjectSaleDetailsResponse
    {
        public StageDTO Stage { get; set; }
        public List<PaymentCurrencyDTO> PaymentCurrencies { get; set; }
        public decimal SoldTokens { get; set; }
        public string CompanyWalletAddress { get; set; }
        public string CompanyWalletNetwork { get; set; }
    }
}