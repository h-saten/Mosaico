using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Domain.Wallet.EntityConfigurations.Staking
{
    public class StakingClaimHistoryEntityConfiguration : EntityConfigurationBase<StakingClaimHistory>
    {
        protected override string TableName => Constants.Tables.StakingClaimHistory;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<StakingClaimHistory> builder)
        {
            base.Configure(builder);
            builder.HasOne(s => s.StakingPair).WithMany().HasForeignKey(t => t.StakingPairId);
        }
    }
}