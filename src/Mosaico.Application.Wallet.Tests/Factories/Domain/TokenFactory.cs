using FizzWare.NBuilder;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Tests.Factories.Domain
{
    internal static class TokenFactory
    {
        public static Token Create(string network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var token = Builder<Token>.CreateNew().Build();
            token.Network = network;
            return token;
        }

        public static Token CreateToken(this IWalletDbContext context, string network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum)
        {
            var token = Create(network);
            context.Tokens.Add(token);
            return token;
        }
    }
}