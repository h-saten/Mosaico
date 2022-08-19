using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class InvestmentPackageLogoEntityConfiguration : IEntityTypeConfiguration<InvestmentPackageLogo>
    {
        public void Configure(EntityTypeBuilder<InvestmentPackageLogo> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.InvestmentPackageId).IsRequired();
            builder.Property(p => p.PageId).IsRequired();
            builder.HasIndex(p => new { p.PageId, p.InvestmentPackageId }).IsUnique(true);
        }
    }
}