using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Checkout.MosaicoWalletCheckoutContext
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class MosaicoWalletCheckoutContextQuery : IRequest<MosaicoWalletCheckoutContextQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
    }
}