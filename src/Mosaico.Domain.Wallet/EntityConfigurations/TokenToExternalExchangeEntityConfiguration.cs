using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TokenToExternalExchangeEntityConfiguration : IEntityTypeConfiguration<TokenToExternalExchange>
    {
        public void Configure(EntityTypeBuilder<TokenToExternalExchange> builder)
        {
            builder.ToTable(Constants.Tables.TokenToExternalExchanges, Constants.Schema);
            builder.HasKey(e => new { e.TokenId, e.ExternalExchangeId });
            builder.HasOne(t => t.Token).WithMany(token => token.Exchanges).HasForeignKey(t => t.TokenId);
            builder.HasOne(t => t.ExternalExchange).WithMany(e => e.Tokens).HasForeignKey(e => e.ExternalExchangeId);
        }
    }
}