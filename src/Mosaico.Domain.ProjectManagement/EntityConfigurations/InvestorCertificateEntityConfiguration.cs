using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class InvestorCertificateEntityConfiguration : EntityConfigurationBase<InvestorCertificate>
    {
        protected override string TableName => Constants.Tables.InvestorCertificate;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<InvestorCertificate> builder)
        {
            base.Configure(builder);
        }
    }
}