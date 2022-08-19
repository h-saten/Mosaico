using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class PaymentCurrencyConfiguration : EntityConfigurationBase<PaymentCurrency>
    {
        protected override string TableName => Constants.Tables.PaymentCurrency;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<PaymentCurrency> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Chain).IsRequired();
            builder.Property(t => t.Ticker).IsRequired();
            builder.HasIndex(t => new {t.Name, t.Ticker, t.Chain}).IsUnique();
            
            builder.HasData(new PaymentCurrency
            {
                Id = new Guid("8c53a7ba-0d71-47f7-8a80-d1534656be0c"),
                Name = "Ethereum",
                Ticker = "ETH",
                Chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
                NativeChainCurrency = true,
                DecimalPlaces = 18
            });
            builder.HasData(new PaymentCurrency
            {
                Id = new Guid("13fb17b8-5979-4258-af02-423a58c79878"),
                Name = "Tether USD",
                Ticker = "USDT",
                Chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
                ContractAddress = "0xdAC17F958D2ee523a2206206994597C13D831ec7",
                DecimalPlaces = 6
            });
            builder.HasData(new PaymentCurrency
            {
                Id = new Guid("f0e5097d-383d-420f-91f0-0fc7a9d2770e"),
                Name = "USD Coin",
                Ticker = "USDC",
                Chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
                ContractAddress = "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48",
                DecimalPlaces = 6
            });
            // builder.HasData(PaymentCurrency.AddNativeCurrency("Matic", "MATIC", Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Polygon));
            // builder.HasData(PaymentCurrency.AddStableCoin("Tether USD", "USDT", Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Polygon, "0xc2132D05D31c914a87C6611C10748AEb04B58e8F"));
            // builder.HasData(PaymentCurrency.AddStableCoin("USD Coin", "USDC", Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Polygon, "0x2791Bca1f2de4661ED88A30C99A7a9449Aa84174"));
        }
    }
}