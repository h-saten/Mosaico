using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Domain.Wallet.EntityConfigurations.Staking
{
    public class StakingPairEntityConfiguration : EntityConfigurationBase<StakingPair>
    {
        protected override string TableName => Constants.Tables.StakingPairs;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StakingPair> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Token).WithMany().HasForeignKey(t => t.TokenId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(t => t.StakingToken).WithMany().HasForeignKey(t => t.StakingTokenId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(t => t.StakingVersion).HasDefaultValue("v1");
            builder.HasOne(t => t.StakingRegulation).WithOne(t => t.StakingPair)
                .HasForeignKey<StakingPair>(t => t.StakingRegulationId).IsRequired(false);
        }
    }
}