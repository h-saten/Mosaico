using MediatR;

namespace Mosaico.Application.Wallet.Queries.WalletTransactions
{
    public class WalletTransactionsQuery : IRequest<WalletTransactionsResponse>
    {
        public string WalletAddress { get; set; }
        public string Network { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 30;
        public string PageKey { get; set; }
    }
}