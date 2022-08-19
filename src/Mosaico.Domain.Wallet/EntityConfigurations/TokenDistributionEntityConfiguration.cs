using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TokenDistributionEntityConfiguration : EntityConfigurationBase<TokenDistribution>
    {
        protected override string TableName => Constants.Tables.TokenDistributions;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<TokenDistribution> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Token).WithMany(t => t.Distributions).HasForeignKey(t => t.TokenId);
            builder.Property(x => x.TokenAmount).HasPrecision(36, 18);
        }
    }
}