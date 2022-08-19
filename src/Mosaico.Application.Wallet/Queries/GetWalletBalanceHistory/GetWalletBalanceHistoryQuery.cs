using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetWalletBalanceHistory
{
    [Cache("{{Network}}_{{WalletAddress}}", ExpirationInMinutes = 3)]
    public class GetWalletBalanceHistoryQuery : IRequest<GetWalletBalanceHistoryQueryResponse>
    {
        public string WalletAddress { get; set; }
        public string Network { get; set; }
    }
}