using FizzWare.NBuilder;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Tests.Factories.Domain
{
    internal static class WalletFactory
    {
        public static Mosaico.Domain.Wallet.Entities.Wallet Create(
            string userId, 
            string accountAddress = "0x4d45024afdD0B862448865eAB591d35EE3952293",
            string network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var wallet = Builder<Mosaico.Domain.Wallet.Entities.Wallet>.CreateNew().Build();
            wallet.UserId = userId;
            wallet.Network = network;
            wallet.AccountAddress = accountAddress;
            return wallet;
        }

        public static Mosaico.Domain.Wallet.Entities.Wallet CreateWallet(
            this IWalletDbContext context,
            string userId, 
            string accountAddress = "0x4d45024afdD0B862448865eAB591d35EE3952293",
            string network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var wallet = Create(userId, accountAddress, network);
            context.Wallets.Add(wallet);
            return wallet;
        }
    }
}