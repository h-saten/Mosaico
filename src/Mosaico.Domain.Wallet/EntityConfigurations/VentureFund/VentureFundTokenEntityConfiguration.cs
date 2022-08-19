using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities.Fund;

namespace Mosaico.Domain.Wallet.EntityConfigurations.VentureFund
{
    public class VentureFundTokenEntityConfiguration : EntityConfigurationBase<VentureFundToken>
    {
        protected override string TableName => Constants.VentureFundTables.VentureFundTokens;
        protected override string Schema => Constants.FundSchema;
        public override void Configure(EntityTypeBuilder<VentureFundToken> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.VentureFund).WithMany(f => f.Tokens).HasForeignKey(t => t.VentureFundId);
        }
    }
}