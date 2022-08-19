using System;
using FizzWare.NBuilder;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Tests.Factories.Domain
{
    internal static class PaymentCurrencyFactory
    {
        public static PaymentCurrency Create(
            string name,
            string ticker,
            string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var paymentCurrency = Builder<PaymentCurrency>.CreateNew().Build();
            paymentCurrency.Id = Guid.NewGuid();
            paymentCurrency.Name = name;
            paymentCurrency.Ticker = ticker;
            paymentCurrency.Chain = chain;
            paymentCurrency.ContractAddress = EthereumAddressFaker.Generate();
            return paymentCurrency;
        }

        public static PaymentCurrency CreateUsdt(
            this IWalletDbContext context,
            string chain = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var currency = Create("Tether", "USDT", chain);
            context.PaymentCurrencies.Add(currency);
            return currency;
        }
    }
}