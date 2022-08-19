using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class AirdropCampaignEntityConfiguration : EntityConfigurationBase<AirdropCampaign>
    {
        protected override string TableName => Constants.Tables.AirdropCampaigns;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<AirdropCampaign> builder)
        {
            base.Configure(builder);
            builder.HasIndex(p => p.Slug).IsUnique(false);
            builder.HasIndex(p => new {p.Slug, p.ProjectId}).IsUnique(true);
            builder.HasMany(a => a.Participants).WithOne(p => p.AirdropCampaign)
                .HasForeignKey(a => a.AirdropCampaignId);
            builder.HasOne(c => c.Project).WithMany(p => p.AirdropCampaigns).HasForeignKey(c => c.ProjectId);
        }
    }
}