using System;
using System.Collections.Generic;

namespace Mosaico.Application.ProjectManagement.DTOs.TokenPage
{
    public class InvestmentPackageDTO
    {
        public Guid Id { get; set; }
        public decimal TokenAmount { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public List<string> Benefits { get; set; }
    }
}