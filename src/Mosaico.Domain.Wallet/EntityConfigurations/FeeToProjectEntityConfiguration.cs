using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class FeeToProjectEntityConfiguration : EntityConfigurationBase<FeeToProject>
    {
        protected override string TableName => Constants.Tables.FeeToProjects;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<FeeToProject> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => new { t.ProjectId, t.StageId}).IsUnique();
        }
    }
}