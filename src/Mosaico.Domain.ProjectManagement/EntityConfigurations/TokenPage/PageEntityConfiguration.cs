using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class PageEntityConfiguration : EntityConfigurationBase<Page>
    {
        protected override string TableName => Domain.ProjectManagement.Constants.Tables.Pages;
        protected override string Schema => Domain.ProjectManagement.Constants.Schema;

        public override void Configure(EntityTypeBuilder<Page> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Project).WithOne(p => p.Page)
                .HasForeignKey<Project>(p => p.PageId).IsRequired(false);
            builder.HasMany(p => p.Faqs).WithOne(p => p.Page).HasForeignKey(f => f.PageId);
            builder.HasMany(p => p.PageCovers).WithOne(c => c.Page).HasForeignKey(c => c.PageId);
            builder.HasMany(p => p.SocialMediaLinks).WithOne(sml => sml.Page).HasForeignKey(sml => sml.PageId);
            builder.HasMany(p => p.TeamMembers).WithOne(p => p.Page).HasForeignKey(p => p.PageId);
            builder.HasMany(p => p.PagePartners).WithOne(p => p.Page).HasForeignKey(p => p.PageId);
            builder.HasOne(p => p.ShortDescription).WithOne(s => s.Page).HasForeignKey<ShortDescription>(s => s.PageId);
            builder.HasMany(p => p.InvestmentPackages).WithOne(p => p.Page).HasForeignKey(p => p.PageId);
        }
    }
}