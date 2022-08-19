using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Tests.Factories.Domain
{
    internal static class TransactionStatusFactory
    {
        public static TransactionStatus Pending()
            => new TransactionStatus( Mosaico.Domain.Wallet.Constants.TransactionStatuses.Pending, "Pending");
        
        public static TransactionStatus Confirmed()
            => new TransactionStatus( Mosaico.Domain.Wallet.Constants.TransactionStatuses.Confirmed, "Confirmed");

        public static TransactionStatus CreatePending(this IWalletDbContext context)
        {
            var status = Pending();
            context.TransactionStatuses.Add(status);
            return status;
        }
        
        public static TransactionStatus CreateConfirmed(this IWalletDbContext context)
        {
            var status = Confirmed();
            context.TransactionStatuses.Add(status);
            return status;
        }
    }
}