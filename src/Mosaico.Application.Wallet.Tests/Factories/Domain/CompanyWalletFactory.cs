using System;
using FizzWare.NBuilder;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Tests.Factories.Domain
{
    internal static class CompanyWalletFactory
    {
        public static Mosaico.Domain.Wallet.Entities.CompanyWallet Create(
            Guid companyId, 
            string accountAddress = "0x4d45024afdD0B862448865eAB591d35EE3952294",
            string network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var companyWallet = Builder<Mosaico.Domain.Wallet.Entities.CompanyWallet>.CreateNew().Build();
            companyWallet.CompanyId = companyId;
            companyWallet.Network = network;
            companyWallet.AccountAddress = accountAddress;
            return companyWallet;
        }

        public static Mosaico.Domain.Wallet.Entities.CompanyWallet CreateCompanyWallet(
            this IWalletDbContext context,
            Guid companyId, 
            string accountAddress = "0x4d45024afdD0B862448865eAB591d35EE3952294",
            string network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var companyWallet = Create(companyId, accountAddress, network);
            context.CompanyWallets.Add(companyWallet);
            return companyWallet;
        }
    }
}