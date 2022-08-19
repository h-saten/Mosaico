using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class StageEntityConfiguration : EntityConfigurationBase<Stage>
    {
        protected override string TableName => Constants.Tables.Stage;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<Stage> builder)
        {
            base.Configure(builder);
            builder.HasOne(s => s.Status).WithMany().HasForeignKey(s => s.StatusId);
            builder.Property(s => s.Name).IsRequired();
            builder.HasOne(s => s.Project).WithMany(s => s.Stages).HasForeignKey(s => s.ProjectId);
            builder.Property(x => x.TokensSupply).HasPrecision(36, 18);
            builder.Property(x => x.TokenPrice).HasPrecision(36, 18);
            builder.Property(x => x.TokenPriceNativeCurrency).HasPrecision(36, 18);
            builder.Property(x => x.MinimumPurchase).HasPrecision(36, 18);
            builder.Property(x => x.MaximumPurchase).HasPrecision(36, 18);
        }
    }
}