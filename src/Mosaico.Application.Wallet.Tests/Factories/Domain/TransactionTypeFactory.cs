using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Tests.Factories.Domain
{
    internal static class TransactionTypeFactory
    {
        public static TransactionType Purchase()
            => new ( Mosaico.Domain.Wallet.Constants.TransactionType.Purchase, "Purchase");

        public static TransactionType CreatePurchase(this IWalletDbContext context)
        {
            var status = Purchase();
            context.TransactionType.Add(status);
            return status;
        }
    }
}