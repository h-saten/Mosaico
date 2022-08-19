using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class ArticlePhotoEntityConfiguration : IEntityTypeConfiguration<ArticlePhoto>
    {
        public void Configure(EntityTypeBuilder<ArticlePhoto> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.ArticleId).IsRequired();
            builder.HasIndex(p => p.ArticleId).IsUnique(true);
        }
    }
}