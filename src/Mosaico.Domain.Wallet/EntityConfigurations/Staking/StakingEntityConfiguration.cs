using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;

namespace Mosaico.Domain.Wallet.EntityConfigurations.Staking
{
    public class StakingEntityConfiguration : EntityConfigurationBase<Entities.Staking.Staking>
    {
        protected override string TableName => Constants.Tables.Stakings;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Entities.Staking.Staking> builder)
        {
            base.Configure(builder);
            builder.HasOne(s => s.StakingPair).WithMany().HasForeignKey(t => t.StakingPairId);
        }
    }
}