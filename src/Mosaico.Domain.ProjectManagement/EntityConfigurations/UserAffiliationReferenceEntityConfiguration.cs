using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class UserAffiliationReferenceEntityConfiguration : EntityConfigurationBase<UserAffiliationReference>
    {
        protected override string TableName => Constants.Tables.UserAffiliationReferences;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<UserAffiliationReference> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.UserAffiliation).WithMany().HasForeignKey(t => t.UserAffiliationId);
        }
    }
}