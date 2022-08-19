using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.Affiliation
{
    public class ProjectAffiliationEntityConfiguration : EntityConfigurationBase<ProjectAffiliation>
    {
        protected override string TableName => Constants.Tables.ProjectAffiliations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectAffiliation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Project)
                .WithOne(p => p.ProjectAffiliation).HasForeignKey<ProjectAffiliation>(t => t.ProjectId);
        }
    }
}