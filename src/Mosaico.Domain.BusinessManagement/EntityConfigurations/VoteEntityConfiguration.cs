using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class VoteEntityConfiguration : EntityConfigurationBase<Vote>
    {
        protected override string TableName => Constants.Tables.Votes;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Vote> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Proposal).WithMany(p => p.Votes).HasForeignKey(t => t.ProposalId);
            builder.HasIndex(t => new {t.ProposalId, t.VotedByAddress}).IsUnique();
        }
    }
}