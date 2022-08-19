using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class ExchangeRateEntityConfiguration : EntityConfigurationBase<ExchangeRate>
    {
        protected override string TableName => Constants.Tables.ExchangeRates;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            base.Configure(builder);
            builder.HasIndex(e => e.CreatedAt);
            builder.Property(x => x.Rate).HasPrecision(36, 18);
        }
    }
}