using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class PageCoverEntityConfiguration : IEntityTypeConfiguration<PageCover>
    {
        public void Configure(EntityTypeBuilder<PageCover> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.PageId).IsRequired();
            builder.HasIndex(p => p.PageId).IsUnique(true);
        }
    }
}