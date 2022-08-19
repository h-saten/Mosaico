using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class PartnerEntityConfiguration : EntityConfigurationBase<PagePartners>
    {
        protected override string TableName => Constants.Tables.PagePartners;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<PagePartners> builder)
        {
            base.Configure(builder);
        }
    }
}