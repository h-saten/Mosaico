using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class AboutEntityConfiguration : EntityConfigurationBase<About>
    {
        protected override string TableName => Domain.ProjectManagement.Constants.Tables.Abouts;
        protected override string Schema => Domain.ProjectManagement.Constants.Schema;

        public override void Configure(EntityTypeBuilder<About> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Page).WithOne(p => p.About)
                .HasForeignKey<Page>(p => p.AboutId).IsRequired(false);
            
            builder.HasOne(f => f.Content)
            .WithOne(c => c.About)
            .HasForeignKey<AboutContent>(fc => fc.AboutId);
        }
    }

    public class AboutContentTranslationEntityConfiguration : EntityConfigurationBase<AboutContentTranslation>
    {
        protected override string TableName => Constants.Tables.AboutContentTranslation;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<AboutContentTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.AboutContent)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }


}