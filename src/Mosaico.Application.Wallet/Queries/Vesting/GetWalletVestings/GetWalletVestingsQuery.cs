using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Vesting.GetWalletVestings
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetWalletVestingsQuery : IRequest<GetWalletVestingsQueryResponse>
    {
        public string UserId { get; set; }
        public string Network { get; set; }
    }
}