using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class InvestorCertificateBackgroundEntityConfiguration : EntityConfigurationBase<InvestorCertificateBackground>
    {
        protected override string TableName => Constants.Tables.InvestorCertificateBackground;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<InvestorCertificateBackground> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Language).IsRequired();
            builder.Property(p => p.Url).IsRequired();
            builder.Property(p => p.InvestorCertificateId).IsRequired();
        }
    }
}