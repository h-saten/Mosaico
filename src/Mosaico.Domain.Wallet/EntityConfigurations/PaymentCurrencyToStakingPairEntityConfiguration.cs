using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class PaymentCurrencyToStakingPairEntityConfiguration : EntityConfigurationBase<PaymentCurrencyToStakingPair>
    {
        protected override string TableName => Constants.Tables.PaymentCurrencyToStakingPairs;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<PaymentCurrencyToStakingPair> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.StakingPair).WithMany(t => t.PaymentCurrencies)
                .HasForeignKey(t => t.StakingPairId);
            builder.HasOne(t => t.PaymentCurrency).WithMany().HasForeignKey(t => t.PaymentCurrencyId);
        }
    }
}