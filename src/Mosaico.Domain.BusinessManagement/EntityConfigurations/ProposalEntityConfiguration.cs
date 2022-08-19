using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class ProposalEntityConfiguration : EntityConfigurationBase<Proposal>
    {
        protected override string TableName => Constants.Tables.Proposals;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Proposal> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Company)
                .WithMany(c => c.Proposals)
                .HasForeignKey(t => t.CompanyId);
        }
    }
}