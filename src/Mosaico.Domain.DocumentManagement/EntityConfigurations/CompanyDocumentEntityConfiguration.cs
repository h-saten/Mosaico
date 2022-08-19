using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class CompanyDocumentEntityConfiguration : IEntityTypeConfiguration<CompanyDocument>
    {
        public void Configure(EntityTypeBuilder<CompanyDocument> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.CompanyId).IsRequired();
        }
    }
}
