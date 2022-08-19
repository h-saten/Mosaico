using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class TokenLogoEntityConfiguration : IEntityTypeConfiguration<TokenLogo>
    {
        public void Configure(EntityTypeBuilder<TokenLogo> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.TokenId).IsRequired();
            builder.HasIndex(p => p.TokenId).IsUnique(true);
        }
    }
}