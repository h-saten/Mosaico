using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class CompanySubscriberEntityConfiguration : EntityConfigurationBase<CompanySubscriber>
    {
        protected override string TableName => Constants.Tables.CompanySubscribers;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<CompanySubscriber> builder)
        {
            base.Configure(builder);
            builder.HasOne(c => c.Company)
                .WithMany(c => c.CompanySubscribers).HasForeignKey(t => t.CompanyId);
            builder.HasIndex(c => new {c.Email, c.CompanyId}).IsUnique(true);
        }
    }
}