using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectNewsletterSubscriberEntityConfiguration : EntityConfigurationBase<ProjectNewsletterSubscription>
    {
        protected override string TableName => Constants.Tables.ProjectNewsletterSubscriptions;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectNewsletterSubscription> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Project).WithMany(p => p.ProjectNewsletterSubscriptions)
                .HasForeignKey(t => t.ProjectId);
            builder.HasIndex(p => new { p.Email, p.ProjectId }).IsUnique(true);
        }
    }
}