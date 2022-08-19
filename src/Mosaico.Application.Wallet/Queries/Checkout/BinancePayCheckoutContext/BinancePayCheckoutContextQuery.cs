using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Checkout.BinancePayCheckoutContext
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class BinancePayCheckoutContextQuery : IRequest<BinancePayCheckoutContextQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
    }
}