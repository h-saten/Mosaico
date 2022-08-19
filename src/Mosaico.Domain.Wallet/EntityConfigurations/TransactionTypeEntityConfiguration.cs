using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TransactionTypeEntityConfiguration : EntityConfigurationBase<TransactionType>
    {
        protected override string TableName => Constants.Tables.TransactionTypes;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<TransactionType> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.Key).IsUnique(true);
            builder.HasData(new TransactionType(Constants.TransactionType.Deposit, "Deposit") {Id = new Guid("b0398047-84cf-4264-9cc5-4bd2c839eaed")});
            builder.HasData(new TransactionType(Constants.TransactionType.Purchase, "Purchase") {Id = new Guid("077cc88b-5c2a-4fce-9f41-9c6a3fea38f7")});
            builder.HasData(new TransactionType(Constants.TransactionType.Exchange, "Exchange") {Id = new Guid("7fcd586d-10b8-4e5b-a897-ceedc91510e6")});
            builder.HasData( new TransactionType(Constants.TransactionType.Withdrawal, "Withdrawal") {Id = new Guid("06b797db-a303-4cfe-954a-9d18d55a4f3a")});
            builder.HasData( new TransactionType(Constants.TransactionType.Transfer, "Transfer") {Id = new Guid("799CAC01-4E82-4B82-9A22-2D633C55DF6B")});
        }
    }
}