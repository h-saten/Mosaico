using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class AirdropParticipantConfiguration : EntityConfigurationBase<AirdropParticipant>
    {
        protected override string TableName => Constants.Tables.AirdropParticipants;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<AirdropParticipant> builder)
        {
            base.Configure(builder);
            builder.HasIndex(p => new {p.Email, p.AirdropCampaignId}).IsUnique(true);
        }
    }
}