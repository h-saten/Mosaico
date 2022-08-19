using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectStatusEntityConfiguration : EntityConfigurationBase<ProjectStatus>
    {
        protected override string TableName => Constants.Tables.ProjectStatuses;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectStatus> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Key).IsRequired();
            builder.HasIndex(p => p.Key).IsUnique();
            builder.HasData(new ProjectStatus(Constants.ProjectStatuses.New, "New"){Id = new Guid("74246a47-a93d-4713-b8c7-4f51263947ce"), Order = 1});
            builder.HasData(new ProjectStatus(Constants.ProjectStatuses.UnderReview, "Under review"){Id = new Guid("5730abcf-134b-4116-b186-0e1f54c1d1c6"), Order = 4});
            builder.HasData(new ProjectStatus(Constants.ProjectStatuses.Approved, "Approved"){Id = new Guid("9aa58972-d2c8-467d-a162-7ea773e5aded"), Order = 3});
            builder.HasData(new ProjectStatus(Constants.ProjectStatuses.Declined, "Declined"){Id = new Guid("7529ec5c-5351-44f9-bea6-e89d27a3bd23"), Order = 1});
            builder.HasData(new ProjectStatus(Constants.ProjectStatuses.InProgress, "In Progress"){Id = new Guid("6d150791-925f-4c7e-8f9a-87d31e3aa061"), Order = 5});
            builder.HasData(new ProjectStatus(Constants.ProjectStatuses.Closed, "Closed"){Id = new Guid("fc4982ea-5b11-4a75-a2d4-4d192ba42848"), Order = 2});
        }
    }
}