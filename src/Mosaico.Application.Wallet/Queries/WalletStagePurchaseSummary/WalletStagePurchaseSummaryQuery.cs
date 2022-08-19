using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.WalletStagePurchaseSummary
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class WalletStagePurchaseSummaryQuery : IRequest<WalletStagePurchaseSummaryResponse>
    {
        public string UserId { get; set; }
        public string Network { get; set; }
        public string StageId { get; set; }
    }
}