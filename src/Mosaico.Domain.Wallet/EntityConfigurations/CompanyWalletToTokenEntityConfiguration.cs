using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class CompanyWalletToTokenEntityConfiguration: IEntityTypeConfiguration<CompanyWalletToToken>
    {
        public void Configure(EntityTypeBuilder<CompanyWalletToToken> builder)
        {
            builder.ToTable(Constants.Tables.CompanyWalletToToken, Constants.Schema);
            builder.HasKey(t => new { t.TokenId, t.CompanyWalletId });
            builder.HasOne(w => w.Token).WithMany(t => t.CompanyWallets);
            builder.HasOne(w => w.CompanyWallet).WithMany(w => w.Tokens);
        } 
    }
}