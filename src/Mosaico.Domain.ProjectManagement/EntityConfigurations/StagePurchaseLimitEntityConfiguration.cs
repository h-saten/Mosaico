using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class StagePurchaseLimitEntityConfiguration : EntityConfigurationBase<StagePurchaseLimit>
    {
        protected override string TableName => Constants.Tables.StagePurchaseLimits;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StagePurchaseLimit> builder)
        {
            base.Configure(builder);
            builder.HasOne(s => s.Stage).WithMany(s => s.PurchaseLimits).HasForeignKey(t => t.StageId);
        }
    }
}