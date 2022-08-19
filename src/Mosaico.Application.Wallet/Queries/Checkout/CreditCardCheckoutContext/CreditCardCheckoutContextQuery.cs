using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Checkout.CreditCardCheckoutContext
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class CreditCardCheckoutContextQuery : IRequest<CreditCardCheckoutContextQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }       
    }
}