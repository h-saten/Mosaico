using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class ProjectDocumentEntityConfiguration : IEntityTypeConfiguration<ProjectDocument>
    {
        public void Configure(EntityTypeBuilder<ProjectDocument> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.ProjectId).IsRequired();
        }
    }
}
