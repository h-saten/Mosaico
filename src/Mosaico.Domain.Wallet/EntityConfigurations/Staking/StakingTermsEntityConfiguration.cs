using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Domain.Wallet.EntityConfigurations.Staking
{
    public class StakingTermsEntityConfiguration : EntityConfigurationBase<StakingTerms>
    {
        protected override string TableName => Constants.Tables.StakingTerms;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StakingTerms> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.StakingPair).WithOne(p => p.StakingTerms)
                .HasForeignKey<StakingTerms>(t => t.StakingPairId);
        }
    }

    public class StakingTermsTranslationEntityConfiguration : EntityConfigurationBase<StakingTermsTranslation>
    {
        protected override string TableName => Constants.Tables.StakingTermsTranslations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StakingTermsTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.StakingTerms)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
}