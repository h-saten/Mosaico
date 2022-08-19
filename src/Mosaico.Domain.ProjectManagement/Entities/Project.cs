using System;
using System.Collections.Generic;
using System.Linq;
using Mosaico.Domain.Base;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.Domain.ProjectManagement.Entities.Ratings;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class Project : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string SlugInvariant { get; set; }
        public string TitleInvariant { get; set; }
        public string LogoUrl { get; set; }
        public virtual ProjectStatus Status { get; set; }
        public Guid StatusId { get; set; }
        public Guid? TokenId { get; set; }
        public virtual List<Stage> Stages { get; set; } = new List<Stage>();
        public Guid? CompanyId { get; set; }
        public virtual Crowdsale Crowdsale { get; set; }
        public bool IsVisible { get; set; }
        public Guid? CrowdsaleId { get; set; }
        public Guid? LegacyId { get; set; }
        public virtual List<ProjectMember> Members { get; set; } = new List<ProjectMember>();
        public virtual List<PagePartners> PagePartners { get; set; } = new List<PagePartners>();
        public virtual List<ProjectNewsletterSubscription> ProjectNewsletterSubscriptions { get; set; } = new List<ProjectNewsletterSubscription>();
        public Guid? PageId { get; set; }
        public virtual Page Page { get; set; }
        public virtual InvestorCertificate InvestorCertificate { get; set; }
        public virtual List<Document> Documents { get; set; } = new List<Document>();
        public virtual List<Article> Articles { get; set; } = new List<Article>();
        public virtual List<PaymentMethod> PaymentMethods { get; set; } = new();
        public virtual List<ProjectPaymentMethod> ProjectPaymentMethods { get; set; } = new();
        public virtual List<AirdropCampaign> AirdropCampaigns { get; set; } = new();
        public DateTimeOffset? StartDate { get; set; }
        public int Order { get; set; }
        public bool IsBlockedForEditing { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsVisibleOnLanding { get; set; }
        public bool IsAccessibleViaLink { get; set; }
        public virtual ProjectAffiliation ProjectAffiliation { get; set; }
        public Guid? ProjectAffiliationId { get; set; }
        public virtual List<ProjectLike> Likes { get; set; } = new List<ProjectLike>();
        public virtual List<ProjectVisitors> ProjectVisitors { get; set; } = new List<ProjectVisitors>();
        
        public void SetStatus(ProjectStatus status)
        {
            Status = status;
            StatusId = status.Id;
        }

        public void SetCrowdsale(Crowdsale crowdsale)
        {
            if (crowdsale != null)
            {
                CrowdsaleId = crowdsale.Id;
                Crowdsale = crowdsale;
            }
        }

        public Stage ActiveStage()
        {
            var stages = Stages?.OrderBy(s => s.Order)?.ToList();
            var currentlyRunningStage = stages?.FirstOrDefault(s => s.Status.Key == Constants.StageStatuses.Active) ??
                                        stages?.FirstOrDefault(s => s.Status.Key == Constants.StageStatuses.Pending) ??
                                        stages?.LastOrDefault(s => s.Status.Key == Constants.StageStatuses.Closed);
            return currentlyRunningStage;
        }
        
        public Stage NextStage(int nextOrder)
        {
            return Stages.FirstOrDefault(s => s.Order == nextOrder);
        }
    }
}