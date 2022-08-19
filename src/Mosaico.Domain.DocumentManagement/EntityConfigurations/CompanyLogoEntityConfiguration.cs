using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class CompanyLogoEntityConfiguration : IEntityTypeConfiguration<CompanyLogo>
    {
        public void Configure(EntityTypeBuilder<CompanyLogo> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.CompanyId).IsRequired();
            builder.HasIndex(p => p.CompanyId).IsUnique(true);
        }
    }
}