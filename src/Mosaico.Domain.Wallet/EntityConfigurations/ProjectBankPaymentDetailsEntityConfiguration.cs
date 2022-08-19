using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class ProjectBankPaymentDetailsEntityConfiguration : EntityConfigurationBase<ProjectBankPaymentDetails>
    {
        protected override string TableName => Constants.Tables.ProjectBankPaymentDetails;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectBankPaymentDetails> builder)
        {
            base.Configure(builder);
        }
    }
}