using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectRoleEntityConfiguration : EntityConfigurationBase<ProjectRole>
    {
        protected override string TableName => Constants.Tables.ProjectRoles;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectRole> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Key).IsRequired();
            builder.HasIndex(p => p.Key).IsUnique();
            builder.HasData(new List<ProjectRole>
            {
                new ProjectRole{Id = Guid.Parse("F476FA5C-5483-4C88-8D82-280DC95BA424"), Key = "OWNER", Title = "Owner"},
                new ProjectRole{Id = Guid.Parse("10E1B662-8D5B-4FB8-9632-86CDE1E7F5EC"), Key = "MEMBER", Title = "Member"}
            });
        }
    }
}