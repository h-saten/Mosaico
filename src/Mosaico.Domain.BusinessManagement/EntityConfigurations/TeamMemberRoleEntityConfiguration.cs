using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;
using System;
using System.Collections.Generic;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class TeamMemberRoleEntityConfiguration : EntityConfigurationBase<TeamMemberRole>
    {
        protected override string TableName => Constants.Tables.TeamMemberRole;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<TeamMemberRole> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Key).IsRequired();
            builder.HasIndex(p => p.Key).IsUnique();
            builder.HasData(new List<TeamMemberRole>
            {
                new TeamMemberRole{Id = Guid.Parse("71fa9290-23e6-49e4-8bf9-b0f1083793c8"), Key = "OWNER", Title = "Owner"},
                new TeamMemberRole{Id = Guid.Parse("b13cf50e-6e69-4c9d-a928-dfd340854bf7"), Key = "MEMBER", Title = "Member"}
            });
        }
    }
}