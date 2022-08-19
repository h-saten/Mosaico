using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class DocumentEntityConfiguration: EntityConfigurationBase<Document>
    {
        protected override string TableName => Constants.Tables.Document;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Document> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Company)
                .WithMany(c => c.Documents)
                .HasForeignKey(t => t.CompanyId);
        }
    }
}
