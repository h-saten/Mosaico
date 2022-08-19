using System;
using System.Collections.Generic;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class CompanyListDTO
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public bool IsApproved { get; set; }
        public string LogoUrl { get; set; }
        public string Network { get; set; }
        public bool IsSubscribed { get; set; }
        public string Slug { get; set; }
        public long TotalProposals { get; set; }
        public long OpenProposals { get; set; }
        public List<CompanyListProjectDTO> Projects { get; set; }
    }
}