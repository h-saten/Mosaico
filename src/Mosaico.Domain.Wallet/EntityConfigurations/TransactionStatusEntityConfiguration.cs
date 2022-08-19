using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TransactionStatusEntityConfiguration : EntityConfigurationBase<TransactionStatus>
    {
        protected override string TableName => Constants.Tables.TransactionStatuses;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<TransactionStatus> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.Key).IsUnique(true);
            builder.HasData(new TransactionStatus(Constants.TransactionStatuses.Pending, "Pending"){Id = new Guid("313f94fb-dc91-4013-9a0b-53dd94f133ec")});
            builder.HasData(new TransactionStatus(Constants.TransactionStatuses.Confirmed, "Confirmed"){Id = new Guid("770de2f5-6d2f-4bcf-9b18-73a8eed114ed")});
            builder.HasData(new TransactionStatus(Constants.TransactionStatuses.Canceled, "Canceled"){Id = new Guid("c8605b82-a71a-4c9c-8019-a71154fd103c")});
            builder.HasData(new TransactionStatus(Constants.TransactionStatuses.Failed, "Failed"){Id = new Guid("C154CD29-8D6B-48DF-86A9-A8C979E68A25")});
            builder.HasData(new TransactionStatus(Constants.TransactionStatuses.Refunded, "Refunded"){Id = new Guid("FA2F96AA-E42E-443E-A522-643C94B4F510")});
            builder.HasData(new TransactionStatus(Constants.TransactionStatuses.Expired, "Expired"){Id = new Guid("8D50F357-0853-4AEF-89C9-CEBFE4C2C2E2")});
        }
    }
}