using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class DocumentContentEntityConfiguration : EntityConfigurationBase<DocumentContent>
    {
        protected override string TableName => Constants.Tables.DocumentContents;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<DocumentContent> builder)
        {
            base.Configure(builder);
            builder.HasOne(dc => dc.Document).WithMany().HasForeignKey(dc => dc.DocumentId);
            builder.Property(dc => dc.Language).IsRequired();
            builder.HasIndex(dc => new { dc.DocumentId, dc.Language }).IsUnique();
        }
    }
}
