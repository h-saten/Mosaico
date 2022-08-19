using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class DocumentEntityConfiguration : EntityConfigurationBase<Document>
    {
        protected override string TableName => Constants.Tables.Documents;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Document> builder)
        {
            base.Configure(builder);
            builder.HasOne(d => d.Project).WithMany(p => p.Documents).HasForeignKey(t => t.ProjectId);
        }
    }
}