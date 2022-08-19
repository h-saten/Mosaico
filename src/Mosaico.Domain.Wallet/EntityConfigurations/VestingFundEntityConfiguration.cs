using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class VestingFundEntityConfiguration : EntityConfigurationBase<VestingFund>
    {
        protected override string TableName => Constants.Tables.VestingFunds;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<VestingFund> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Vesting).WithMany(t => t.Funds).HasForeignKey(t => t.VestingId);
        }
    }
}