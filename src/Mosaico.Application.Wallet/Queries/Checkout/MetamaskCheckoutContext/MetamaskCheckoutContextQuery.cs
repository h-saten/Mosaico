using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Checkout.MetamaskCheckoutContext
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class MetamaskCheckoutContextQuery : IRequest<MetamaskCheckoutContextQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
    }
}