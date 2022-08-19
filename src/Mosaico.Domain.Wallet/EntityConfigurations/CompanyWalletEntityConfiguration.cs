using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class CompanyWalletEntityConfiguration : EntityConfigurationBase<CompanyWallet>
    {
        protected override string TableName => Constants.Tables.CompanyWallets;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<CompanyWallet> builder)
        {
            base.Configure(builder);
            builder.Property(w => w.Network).IsRequired();
            builder.Property(w => w.AccountAddress).IsRequired();
            builder.Property(w => w.PrivateKey).IsRequired();
            builder.Property(t => t.PublicKey).IsRequired();
        }
    }
}