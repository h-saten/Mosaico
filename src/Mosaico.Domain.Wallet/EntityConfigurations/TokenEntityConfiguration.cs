using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TokenEntityConfiguration : EntityConfigurationBase<Token>
    {
        protected override string TableName => Constants.Tables.Tokens;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<Token> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Symbol).IsRequired();
            builder.HasOne(t => t.Type).WithMany().HasForeignKey(t => t.TypeId);
            builder.Property(x => x.TotalSupply).HasPrecision(36, 18);
            builder.Property(x => x.TokensLeft).HasPrecision(36, 18);
            builder.HasOne(t => t.Deflation).WithOne(d => d.Token).HasForeignKey<Token>(t => t.DeflationId)
                .IsRequired(false);
            builder.HasMany(t => t.Vestings).WithOne(v => v.Token).HasForeignKey(t => t.TokenId)
                .IsRequired(false);
            builder.HasOne(t => t.Vault).WithOne(v => v.Token)
                .HasForeignKey<Token>(t => t.VaultId).IsRequired(false);
        }
    }
}