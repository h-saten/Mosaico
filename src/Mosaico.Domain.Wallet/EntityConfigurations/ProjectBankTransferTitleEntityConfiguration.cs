using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class ProjectBankTransferTitleEntityConfiguration : EntityConfigurationBase<ProjectBankTransferTitle>
    {
        protected override string TableName => Constants.Tables.ProjectBankTransferTitles;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectBankTransferTitle> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.ProjectBankPaymentDetails)
                .WithMany(p => p.ProjectBankTransferTitles)
                .HasForeignKey(t => t.ProjectBankPaymentDetailsId);
        }
    }
}