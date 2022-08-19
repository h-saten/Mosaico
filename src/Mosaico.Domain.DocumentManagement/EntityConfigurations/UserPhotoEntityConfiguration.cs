using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class UserPhotoEntityConfiguration : IEntityTypeConfiguration<UserPhoto>
    {
        public void Configure(EntityTypeBuilder<UserPhoto> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.UserId).IsRequired();
            builder.HasIndex(p => p.UserId).IsUnique(true);
        }
    }
}