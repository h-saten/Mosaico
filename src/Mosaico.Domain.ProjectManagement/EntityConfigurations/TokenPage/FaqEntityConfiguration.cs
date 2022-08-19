using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Base.Extensions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class FaqEntityConfiguration : EntityConfigurationBase<Faq>
    {
        protected override string TableName => Constants.Tables.Faqs;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Faq> builder)
        {
            base.Configure(builder);
            
            builder.HasOne(f => f.Title)
                .WithOne(t => t.Faq)
                .HasForeignKey<FaqTitle>(ft => ft.FaqId);
            
            builder.HasOne(f => f.Content)
                .WithOne(c => c.Faq)
                .HasForeignKey<FaqContent>(fc => fc.FaqId);
        }
    }

    public class FaqTitleTranslationEntityConfiguration : EntityConfigurationBase<FaqTitleTranslation>
    {
        protected override string TableName => Constants.Tables.FaqTitleTranslations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<FaqTitleTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.FaqTitle)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
    
    public class FaqContentTranslationEntityConfiguration : EntityConfigurationBase<FaqContentTranslation>
    {
        protected override string TableName => Constants.Tables.FaqContentTranslation;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<FaqContentTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.FaqContent)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
    
    
    public class PageCoverTranslationEntityConfiguration : EntityConfigurationBase<PageCoverTranslation>
    {
        protected override string TableName => Constants.Tables.PageCoverTranslations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<PageCoverTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.PageCover)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
    
    public class SocialMediaLinkTranslationEntityConfiguration : EntityConfigurationBase<SocialMediaLinkTranslation>
    {
        protected override string TableName => Constants.Tables.SocialMediaLinkTranslations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<SocialMediaLinkTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.SocialMediaLink)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
}