using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TokenHolderScanJobLogEntityConfiguration : EntityConfigurationBase<TokenHolderScanJobLog>
    {
        protected override string TableName => Constants.Tables.TokenHolderScanJobLogs;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<TokenHolderScanJobLog> builder)
        {
            base.Configure(builder);
            builder.Property(w => w.TokenId).IsRequired();
            builder.Property(w => w.LastFetchedBlock).IsRequired();
        }
    }
}