using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TokenHolderEntityConfiguration : EntityConfigurationBase<TokenHolder>
    {
        protected override string TableName => Constants.Tables.TokenHolders;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<TokenHolder> builder)
        {
            base.Configure(builder);
            builder.Property(w => w.TokenId).IsRequired();
            builder.Property(w => w.WalletAddress).IsRequired();
            builder.Property(w => w.Balance).IsRequired();
            builder.Property(x => x.Balance).HasPrecision(36, 18);
        }
    }
}