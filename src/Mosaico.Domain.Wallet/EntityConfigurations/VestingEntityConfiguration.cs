using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class VestingEntityConfiguration : EntityConfigurationBase<Vesting>
    {
        protected override string TableName => Constants.Tables.Vestings;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Vesting> builder)
        {
            base.Configure(builder);
            builder.HasMany(s => s.Funds).WithOne(vf => vf.Vesting).HasForeignKey(vf => vf.VestingId);
            builder.Property(s => s.TokenId).IsRequired();
            builder.HasIndex(v => v.TokenId).IsUnique(false);
            builder.HasOne(v => v.Token).WithMany(t => t.Vestings).HasForeignKey(v => v.TokenId);
            builder.HasOne(v => v.Vault).WithMany().HasForeignKey(v => v.VaultId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}