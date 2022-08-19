using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class Page : EntityBase
    {
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<Faq> Faqs { get; set; } = new List<Faq>();
        public Guid? AboutId { get; set; }
        public virtual About About { get; set; }
        public virtual List<PageCover> PageCovers { get; set; } = new List<PageCover>();
        public virtual List<SocialMediaLink> SocialMediaLinks { get; set; } = new List<SocialMediaLink>();
        public virtual List<PageTeamMember> TeamMembers { get; set; } = new List<PageTeamMember>();
        public virtual List<PagePartners> PagePartners { get; set; } = new List<PagePartners>();
        public virtual ShortDescription ShortDescription { get; set; }
        public Guid ShortDescriptionId { get; set; }
        public virtual List<InvestmentPackage> InvestmentPackages { get; set; } = new List<InvestmentPackage>();
        public virtual List<PageIntroVideos> PageIntroVideos { get; set; } = new List<PageIntroVideos>();
        public virtual List<Script> Scripts { get; set; } = new List<Script>();
        public virtual List<PageReview> PageReviews { get; set; } = new List<PageReview>();
    }
}