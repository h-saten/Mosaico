using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class TeamMemberEntityConfiguration : EntityConfigurationBase<PageTeamMember>
    {
        protected override string TableName => Constants.Tables.PageMembers;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<PageTeamMember> builder)
        {
            base.Configure(builder);
        }
    }
}