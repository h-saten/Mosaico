using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;

namespace Mosaico.Domain.Wallet.EntityConfigurations.VentureFund
{
    public class VentureFundEntityConfiguration : EntityConfigurationBase<Entities.Fund.VentureFund>
    {
        protected override string TableName => Constants.VentureFundTables.VentureFunds;
        protected override string Schema => Constants.FundSchema;

        public override void Configure(EntityTypeBuilder<Entities.Fund.VentureFund> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.Name).IsUnique(true);
        }
    }
}