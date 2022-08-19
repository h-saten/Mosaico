using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class TeamMemberEntityConfiguration : EntityConfigurationBase<TeamMember>
    {
        protected override string TableName => Constants.Tables.TeamMember;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            base.Configure(builder);
            builder.HasOne(s => s.Company).WithMany(s => s.TeamMembers).HasForeignKey(s => s.CompanyId);
        }
    }
}