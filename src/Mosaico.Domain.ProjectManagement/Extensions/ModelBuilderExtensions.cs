using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.EntityConfigurations;
using Mosaico.Domain.ProjectManagement.EntityConfigurations.Affiliation;
using Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage;
using PartnerEntityConfiguration = Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage.PartnerEntityConfiguration;

namespace Mosaico.Domain.ProjectManagement.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyProjectManagementConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProjectEntityConfiguration());
            builder.ApplyConfiguration(new ProjectStatusEntityConfiguration());
            builder.ApplyConfiguration(new StageStatusEntityConfiguration());
            builder.ApplyConfiguration(new StageEntityConfiguration());
            builder.ApplyConfiguration(new ProjectMemberEntityConfiguration());
            builder.ApplyConfiguration(new ProjectRoleEntityConfiguration());
            builder.ApplyConfiguration(new CrowdsaleEntityConfiguration());
            builder.ApplyConfiguration(new ProjectNewsletterSubscriberEntityConfiguration());
            builder.ApplyConfiguration(new PaymentMethodEntityConfiguration());
            builder.ApplyConfiguration(new ProjectInvestorEntityConfiguration());
            builder.ApplyConfiguration(new StagePurchaseLimitEntityConfiguration());
            builder.ApplyConfiguration(new ProjectLikeEntityConfiguration());
            //Token Page
            builder.ApplyConfiguration(new PageEntityConfiguration());
            builder.ApplyConfiguration(new FaqEntityConfiguration());
            builder.ApplyConfiguration(new TeamMemberEntityConfiguration());
            builder.ApplyConfiguration(new PartnerEntityConfiguration());
            builder.ApplyConfiguration(new PageCoverTranslationEntityConfiguration());
            builder.ApplyConfiguration(new FaqContentTranslationEntityConfiguration());
            builder.ApplyConfiguration(new FaqTitleTranslationEntityConfiguration());
            builder.ApplyConfiguration(new AboutEntityConfiguration());
            builder.ApplyConfiguration(new AboutContentTranslationEntityConfiguration());
            builder.ApplyConfiguration(new ArticleEntityConfiguration());
            builder.ApplyConfiguration(new SocialMediaLinkTranslationEntityConfiguration());
            builder.ApplyConfiguration(new DocumentTypeEntityConfiguration());
            builder.ApplyConfiguration(new DocumentEntityConfiguration());
            builder.ApplyConfiguration(new DocumentTemplateEntityConfiguration());
            builder.ApplyConfiguration(new ShortDescriptionTranslationEntityConfiguration());
            builder.ApplyConfiguration(new ShortDescriptionEntityConfiguration());
            builder.ApplyConfiguration(new InvestmentPackageEntityConfiguration());
            builder.ApplyConfiguration(new InvestmentPackageTranslationEntityConfiguration());
            builder.ApplyConfiguration(new InvestorCertificateEntityConfiguration());
            builder.ApplyConfiguration(new InvestorCertificateBackgroundEntityConfiguration());
            builder.ApplyConfiguration(new ScriptEntityConfiguration());
            builder.ApplyConfiguration(new PageReviewEntityConfiguration());
            //airdrop
            builder.ApplyConfiguration(new AirdropParticipantConfiguration());
            builder.ApplyConfiguration(new AirdropCampaignEntityConfiguration());
            //affiliation
            builder.ApplyConfiguration(new ProjectAffiliationEntityConfiguration());
            builder.ApplyConfiguration(new PartnerEntityConfiguration());
            builder.ApplyConfiguration(new UserAffiliationEntityConfiguration());
            builder.ApplyConfiguration(new PartnerTransactionEntityConfiguration());
            builder.ApplyConfiguration(new UserAffiliationReferenceEntityConfiguration());
        }
    }
}