using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.Statistics.Entities;

namespace Mosaico.Domain.Statistics.EntityConfiguration
{
    public class PurchaseTransactionEntityConfiguration : IEntityTypeConfiguration<PurchaseTransaction>
    {
        protected string TableName => Constants.Tables.PurchaseTransactions;
        protected string Schema => Constants.Schema;
        
        public void Configure(EntityTypeBuilder<PurchaseTransaction> builder)
        {
            builder.ToTable(TableName,Schema);
            builder.HasKey(e => e.Id);
            builder.Property(x => x.TokensAmount).HasPrecision(36, 18);
            builder.Property(x => x.Payed).HasPrecision(36, 18);
            builder.Property(x => x.USDTAmount).HasPrecision(36, 18);
            builder.Property(x => x.TokenId).IsRequired();
            builder.Property(x => x.TransactionId).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Currency).IsRequired();
        }
    }
}