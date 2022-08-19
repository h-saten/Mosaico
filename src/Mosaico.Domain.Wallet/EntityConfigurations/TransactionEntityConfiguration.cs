using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TransactionEntityConfiguration : EntityConfigurationBase<Transaction>
    {
        protected override string TableName => Constants.Tables.Transactions;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Transaction> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.CorrelationId).IsUnique(false);
            builder.HasOne(t => t.Status).WithMany().HasForeignKey(t => t.StatusId);
            builder.HasOne(t => t.Type).WithMany().HasForeignKey(t => t.TypeId);
            builder.Property(x => x.TokenAmount).HasPrecision(36, 18);
            builder.Property(x => x.PayedAmount).HasPrecision(36, 18);
            builder.HasOne(t => t.PaymentCurrency).WithMany().HasForeignKey(t => t.PaymentCurrencyId).IsRequired(false);
            builder.HasOne(t => t.SalesAgent).WithMany(s => s.Transactions).HasForeignKey(t => t.SalesAgentId).IsRequired(false);
        }
    }
}