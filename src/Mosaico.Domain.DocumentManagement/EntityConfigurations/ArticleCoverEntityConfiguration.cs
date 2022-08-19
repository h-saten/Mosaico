using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class ArticleCoverEntityConfiguration : IEntityTypeConfiguration<ArticleCover>
    {
        public void Configure(EntityTypeBuilder<ArticleCover> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.ArticleId).IsRequired();
            builder.HasIndex(p => p.ArticleId).IsUnique(true);
        }
    }
}