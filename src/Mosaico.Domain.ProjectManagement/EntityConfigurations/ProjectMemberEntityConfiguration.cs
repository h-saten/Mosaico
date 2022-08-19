using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectMemberEntityConfiguration : EntityConfigurationBase<ProjectMember>
    {
        protected override string TableName => Constants.Tables.ProjectMembers;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Project).WithMany(p => p.Members).HasForeignKey(p => p.ProjectId);
            builder.HasOne(p => p.Role).WithMany().HasForeignKey(p => p.RoleId);
            builder.HasIndex(m => new {m.Email, m.ProjectId}).IsUnique(true);
            builder.HasIndex(m => m.AuthorizationCode).IsUnique(true);
        }
    }
}