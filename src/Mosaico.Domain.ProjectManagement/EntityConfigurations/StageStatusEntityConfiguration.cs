using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class StageStatusEntityConfiguration : EntityConfigurationBase<StageStatus>
    {
        protected override string TableName => Constants.Tables.StageStatuses;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StageStatus> builder)
        {
            base.Configure(builder);
            builder.HasIndex(s => s.Key).IsUnique();
            builder.HasData(new StageStatus(Constants.StageStatuses.Pending, "Pending"){Id = new Guid("71fa9290-23e6-49e4-8bf9-b0f1083793c7")});
            builder.HasData(new StageStatus(Constants.StageStatuses.Active, "Active"){Id = new Guid("b13cf50e-6e69-4c9d-a928-dfd340854bf9")});
            builder.HasData(new StageStatus(Constants.StageStatuses.Closed, "Closed") {Id = new Guid("a54554e8-e98c-406a-abaf-e383291f029f")});
        }
    }
}