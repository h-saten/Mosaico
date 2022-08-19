using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class WalletToVestingEntityConfiguration : EntityConfigurationBase<WalletToVesting>
    {
        protected override string TableName => Constants.Tables.WalletToVesting;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<WalletToVesting> builder)
        {
            base.Configure(builder);
            builder.HasOne(v => v.Vesting).WithOne().HasForeignKey<WalletToVesting>(t => t.VestingId);
            builder.HasOne(t => t.Wallet).WithMany(w => w.Vestings).HasForeignKey(t => t.WalletId);
        }
    }
}