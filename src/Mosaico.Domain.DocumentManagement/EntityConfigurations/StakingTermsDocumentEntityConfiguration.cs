using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class StakingTermsDocumentEntityConfiguration : IEntityTypeConfiguration<StakingTermsDocument>
    {
        public void Configure(EntityTypeBuilder<StakingTermsDocument> builder)
        {
            builder.HasBaseType<DocumentBase>();
            builder.Property(p => p.StakingPairId).IsRequired();
            builder.HasIndex(p => p.StakingPairId).IsUnique(true);
        }
    }
}