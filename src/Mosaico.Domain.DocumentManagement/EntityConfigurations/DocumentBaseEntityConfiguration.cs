using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.DocumentManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.DocumentManagement.EntityConfigurations
{
    public class DocumentBaseEntityConfiguration : EntityConfigurationBase<DocumentBase>
    {
        protected override string TableName => Constants.Tables.Documents;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<DocumentBase> builder)
        {
            base.Configure(builder);
            builder.Property(d => d.Title).IsRequired();
        }
    }
}
