using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class WalletToTokenEntityConfiguration : IEntityTypeConfiguration<WalletToToken>
    {
        public void Configure(EntityTypeBuilder<WalletToToken> builder)
        {
            builder.ToTable(Constants.Tables.WalletToToken, Constants.Schema);
            builder.HasOne(w => w.Token).WithMany(t => t.Wallets);
            builder.HasOne(w => w.Wallet).WithMany(w => w.Tokens);
            builder.HasKey(t => new {t.TokenId, t.WalletId});
        }
    }
}