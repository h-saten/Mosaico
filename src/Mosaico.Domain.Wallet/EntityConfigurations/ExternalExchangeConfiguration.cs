using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class ExternalExchangeConfiguration : EntityConfigurationBase<ExternalExchange>
    {
        protected override string TableName => Constants.Tables.ExternalExchanges;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ExternalExchange> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.Name).IsUnique(true);
            builder.HasData(new ExternalExchange
            {
                Id = new Guid("BC3B535F-59DB-4512-BA2C-52243CF4790D"),
                Name = "Kanga Exchange",
                Type = ExternalExchangeType.CEX,
                Url = "https://kanga.exchange",
                IsDisabled = false,
                LogoUrl = "/assets/media/logos/kanga_logo.svg"
            });
        }
    }
}