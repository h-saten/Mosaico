using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Checkout.KangaCheckoutContext
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class KangaCheckoutContextQuery : IRequest<KangaCheckoutContextQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }       
    }
}