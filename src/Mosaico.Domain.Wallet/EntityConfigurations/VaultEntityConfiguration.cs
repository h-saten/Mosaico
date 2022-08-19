using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class VaultEntityConfiguration : EntityConfigurationBase<Vault>
    {
        protected override string TableName => Constants.Tables.Vaults;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Vault> builder)
        {
            base.Configure(builder);
            builder.HasOne(v => v.Token)
                .WithOne(t => t.Vault)
                .HasForeignKey<Vault>(v => v.TokenId);
            builder.HasMany(t => t.TokenDistributions)
                .WithOne(d => d.Vault).HasForeignKey(d => d.VaultId);
        }
    }
}