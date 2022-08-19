using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.Affiliation
{
    public class PartnerTransactionEntityConfiguration : EntityConfigurationBase<PartnerTransaction>
    {
        protected override string TableName => Constants.Tables.PartnerTransactions;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<PartnerTransaction> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Partner).WithMany(p => p.PartnerTransactions).HasForeignKey(t => t.PartnerId);
        }
    }
}