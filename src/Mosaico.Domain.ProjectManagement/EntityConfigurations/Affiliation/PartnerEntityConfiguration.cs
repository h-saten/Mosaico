using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.Affiliation
{
    public class PartnerEntityConfiguration : EntityConfigurationBase<Partner>
    {
        protected override string TableName => Constants.Tables.Partners;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Partner> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.ProjectAffiliation).WithMany(a => a.Partners)
                .HasForeignKey(t => t.ProjectAffiliationId);
            builder.HasOne(t => t.UserAffiliation).WithMany(a => a.PartnerAssignments)
                .HasForeignKey(t => t.UserAffiliationId);
        }
    }
}