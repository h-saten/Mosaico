using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Domain.Wallet.EntityConfigurations.Staking
{
    public class StakingRegulationEntityConfiguration : EntityConfigurationBase<StakingRegulation>
    {
        protected override string TableName => Constants.Tables.StakingRegulations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StakingRegulation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.StakingPair).WithOne(p => p.StakingRegulation)
                .HasForeignKey<StakingRegulation>(t => t.StakingPairId);
        }
    }

    public class StakingRegulationTranslationEntityConfiguration : EntityConfigurationBase<StakingRegulationTranslation>
    {
        protected override string TableName => Constants.Tables.StakingRegulationTranslations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StakingRegulationTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.StakingRegulation)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
}