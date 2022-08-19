using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Statistics.Entities;

namespace Mosaico.Domain.Statistics.EntityConfiguration
{
    public class KPIEntityConfiguration : EntityConfigurationBase<KPI>
    {
        protected override string TableName => Constants.Tables.KPIs;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<KPI> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.Key).IsUnique(true);
        }
    }
}