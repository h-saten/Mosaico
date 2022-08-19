using System;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class CompanyListProjectDTO
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string LogoUrl { get; set; }
        public string Title { get; set; }
    }
}