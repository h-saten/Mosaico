using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;
using System;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class DocumentTypeEntityConfiguration : EntityConfigurationBase<DocumentType>
    {
        protected override string TableName => Constants.Tables.DocumentTypes;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            base.Configure(builder);
            builder.HasData(new DocumentType(Constants.DocumentTypes.Whitepaper, "Whitepaper") { Id = new Guid("f604d78a-eba1-4f00-a59c-83e54e16686a"), Order = 1000 });
            builder.HasData(new DocumentType(Constants.DocumentTypes.TermsAndConditions, "Terms And Conditions") { Id = new Guid("0b669ab6-9c3f-4a44-97e4-38d45130a6c3"), Order = 2000});
            builder.HasData(new DocumentType(Constants.DocumentTypes.PrivacyPolicy, "Privacy Policy") { Id = new Guid("f527695b-73b2-405c-b877-45ba6fd47e26"), Order = 3000});

        }
    }
}