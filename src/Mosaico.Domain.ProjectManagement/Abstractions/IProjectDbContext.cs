using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.Domain.ProjectManagement.Entities.Ratings;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using InvestorCertificate = Mosaico.Domain.ProjectManagement.Entities.InvestorCertificate;

namespace Mosaico.Domain.ProjectManagement.Abstractions
{
    public interface IProjectDbContext : IDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Stage> Stages { get; set; }
        DbSet<ProjectStatus> ProjectStatuses { get; set; }
        DbSet<StageStatus> StageStatuses { get; set; }
        DbSet<ProjectRole> Roles { get; set; }
        DbSet<ProjectMember> ProjectMembers { get; set; }
        DbSet<Crowdsale> Crowdsales { get; set; }
        DbSet<Page> TokenPages { get; set; }
        DbSet<Faq> PageFaqs { get; set; }
        DbSet<PageTeamMember> PageTeamMembers { get; set; }
        DbSet<PagePartners> PagePartners { get; set; }
        DbSet<PageCover> PageCovers { get; set; }
        DbSet<PartnerTransaction> PartnerTransactions { get; set; }
        DbSet<SocialMediaLink> PageSocialMediaLinks { get; set; }
        DbSet<FaqContentTranslation> FaqContentTranslations { get; set; }
        DbSet<About> AboutPages { get; set; }
        DbSet<AboutContentTranslation> AboutContentTranslations { get; set; }

        DbSet<FaqTitleTranslation> FaqTitleTranslations { get; set; }
        DbSet<PageCoverTranslation> PageCoverTranslations { get; set; }
        DbSet<SocialMediaLinkTranslation> SocialMediaLinkTranslations { get; set; }
        DbSet<ProjectNewsletterSubscription> ProjectNewsletterSubscriptions { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        DbSet<DocumentType> DocumentTypes { get; set; }
        DbSet<ShortDescription> ShortDescriptions { get; set; }
        DbSet<InvestmentPackage> InvestmentPackages { get; set; }
        DbSet<Article> Articles { get; set; }
        DbSet<PaymentMethod> PaymentMethods { get; set; }
        DbSet<InvestorCertificate> InvestorCertificates { get; set; }
        DbSet<InvestorCertificateBackground> InvestorCertificateBackgrounds { get; set; }
        DbSet<AirdropCampaign> AirdropCampaigns { get; set; }
        DbSet<AirdropParticipant> AirdropParticipants { get; set; }
        DbSet<ProjectInvestor> ProjectInvestors { get; set; }
        DbSet<Partner> Partners { get; set; }
        DbSet<ProjectAffiliation> ProjectAffiliations { get; set; }
        DbSet<UserAffiliation> UserAffiliations { get; set; }
        DbSet<StagePurchaseLimit> StagePurchaseLimits { get; set; }
        DbSet<ProjectLike> ProjectLikes { get; set; }
        DbSet<PageIntroVideos> PageIntroVideos { get; set; }
        DbSet<ProjectVisitors> ProjectVisitors { get; set; }
        DbSet<Script> Scripts { get; set; }
        DbSet<UserAffiliationReference> AffiliationReferences { get; set; }
        DbSet<PageReview> PageReviews { get; set; }
    }
}