using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class PaymentMethodEntityConfiguration : EntityConfigurationBase<PaymentMethod>
    {
        protected override string TableName => Constants.Tables.PaymentMethods;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            base.Configure(builder);
            builder.HasIndex(s => s.Key).IsUnique();
            builder.HasData(new PaymentMethod(Constants.PaymentMethods.MosaicoWallet, "Mosaico Wallet"){Id = new Guid("669ac898-d597-46c0-b9ee-1aaca19d6153")});
            builder.HasData(new PaymentMethod(Constants.PaymentMethods.Metamask, "Metamask"){Id = new Guid("4b21fbdf-9d11-48e2-a770-4a0a29f5d693")});
            builder.HasData(new PaymentMethod(Constants.PaymentMethods.CreditCard, "Credit Card"){Id = new Guid("051d6c04-551b-4f2f-a8b6-bcce1481306f")});
            builder.HasData(new PaymentMethod(Constants.PaymentMethods.KangaExchange, "Kanga Exchange"){Id = new Guid("6cc3cf07-7e91-4077-b0a6-0b546b79a226")});
            builder.HasData(new PaymentMethod(Constants.PaymentMethods.BankTransfer, "Bank Transfer"){Id = new Guid("0A26D7FC-3323-483F-B2F3-A30E033AF7E2")});
            builder.HasData(new PaymentMethod(Constants.PaymentMethods.Binance, "Binance"){Id = new Guid("614005F3-B356-45E8-96A2-B6E2A880240A")});
        }
    }
}