using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetPaymentCurrencyBalance
{
    [Cache("{{UserId}}_{{Network}}", ExpirationInMinutes = 1)]
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetPaymentCurrencyBalanceQuery : IRequest<GetPaymentCurrencyBalanceQueryResponse>
    {
        public string UserId { get; set; }
        public string Network { get; set; }
    }
}