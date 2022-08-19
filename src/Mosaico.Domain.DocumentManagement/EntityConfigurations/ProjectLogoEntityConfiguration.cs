using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class ProjectLogoEntityConfiguration : IEntityTypeConfiguration<ProjectLogo>
    {
        public void Configure(EntityTypeBuilder<ProjectLogo> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.ProjectId).IsRequired();
            builder.HasIndex(p => p.ProjectId).IsUnique(true);
        }
    }
}