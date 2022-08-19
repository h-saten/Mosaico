using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class WalletEntityConfiguration : EntityConfigurationBase<Entities.Wallet>
    {
        protected override string TableName => Constants.Tables.Wallets;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Entities.Wallet> builder)
        {
            base.Configure(builder);
            builder.Property(w => w.Network).IsRequired();
            builder.Property(w => w.AccountAddress).IsRequired();
            builder.Property(w => w.PrivateKey).IsRequired();
            builder.Property(t => t.PublicKey).IsRequired();
        }
    }
}