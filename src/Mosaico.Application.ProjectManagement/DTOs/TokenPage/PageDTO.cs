using System;
using System.Collections.Generic;

namespace Mosaico.Application.ProjectManagement.DTOs.TokenPage
{
    public class PageDTO
    {
        public Guid Id { get; set; }
        public string ShortDescription { get; set; }
        public List<InvestmentPackageDTO> InvestmentPackages { get; set; } = new List<InvestmentPackageDTO>();
        public string CoverUrl { get; set; }
        public string PrimaryColor { get; set; }
        public string CoverColor { get; set; }
        public string SecondaryColor { get; set; }
        public List<SocialMediaLinkDTO> SocialMediaLinks { get; set; } = new List<SocialMediaLinkDTO>();
        public bool HasInvestmentPackages { get; set; }
        public bool HasArticles { get; set; }
        public bool HasFAQs { get; set; }
        public bool HasNFTs { get; set; }
        public bool HasReviews { get; set; }
        public List<ScriptDTO> Scripts { get; set; } = new List<ScriptDTO>();
    }
}