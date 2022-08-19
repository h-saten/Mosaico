using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Extensions;
using System.Collections.Generic;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.Domain.ProjectManagement.Entities.Ratings;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Persistence.SqlServer.Contexts.ProjectContext
{
    public class ProjectContext : DbContextBase<ProjectContext>, IProjectDbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null) :
            base(options, saveChangesCommandInterceptor)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<StageStatus> StageStatuses { get; set; }
        public DbSet<ProjectRole> Roles { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Crowdsale> Crowdsales { get; set; }
        public DbSet<Page> TokenPages { get; set; }
        public DbSet<Faq> PageFaqs { get; set; }
        public DbSet<PageTeamMember> PageTeamMembers { get; set; }
        public DbSet<PagePartners> PagePartners { get; set; }
        public DbSet<PageCover> PageCovers { get; set; }
        public DbSet<SocialMediaLink> PageSocialMediaLinks { get; set; }
        public DbSet<FaqContentTranslation> FaqContentTranslations { get; set; }
        public DbSet<FaqTitleTranslation> FaqTitleTranslations { get; set; }
        public DbSet<PageCoverTranslation> PageCoverTranslations { get; set; }
        public DbSet<SocialMediaLinkTranslation> SocialMediaLinkTranslations { get; set; }
        public DbSet<ProjectNewsletterSubscription> ProjectNewsletterSubscriptions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<ShortDescription> ShortDescriptions { get; set; }
        public DbSet<InvestmentPackage> InvestmentPackages { get; set; }

        public DbSet<AboutContentTranslation> AboutContentTranslations { get; set; }
        public DbSet<About> AboutPages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<InvestorCertificate> InvestorCertificates { get; set; }
        public DbSet<InvestorCertificateBackground> InvestorCertificateBackgrounds { get; set; }
        public DbSet<AirdropCampaign> AirdropCampaigns { get; set; }
        public DbSet<AirdropParticipant> AirdropParticipants { get; set; }
        public DbSet<ProjectInvestor> ProjectInvestors { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<ProjectAffiliation> ProjectAffiliations { get; set; }
        public DbSet<UserAffiliation> UserAffiliations { get; set; }
        public DbSet<PartnerTransaction> PartnerTransactions { get; set; }
        public DbSet<StagePurchaseLimit> StagePurchaseLimits { get; set; }
        public DbSet<ProjectLike> ProjectLikes { get; set; }
        public DbSet<PageIntroVideos> PageIntroVideos { get; set; }
        public DbSet<ProjectVisitors> ProjectVisitors { get; set; }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<UserAffiliationReference> AffiliationReferences { get; set; }
        public DbSet<PageReview> PageReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Domain.ProjectManagement.Constants.Schema);
            modelBuilder.ApplyProjectManagementConfiguration();
            base.OnModelCreating(modelBuilder);
        }
        
        public string ContextName => "core";
    }
}