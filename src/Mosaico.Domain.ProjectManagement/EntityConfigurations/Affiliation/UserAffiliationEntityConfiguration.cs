using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.Affiliation
{
    public class UserAffiliationEntityConfiguration : EntityConfigurationBase<UserAffiliation>
    {
        protected override string TableName => Constants.Tables.UserAffiliations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<UserAffiliation> builder)
        {
            base.Configure(builder);
        }
    }
}