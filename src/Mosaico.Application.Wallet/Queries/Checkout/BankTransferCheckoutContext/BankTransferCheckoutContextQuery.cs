using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Checkout.BankTransferCheckoutContext
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class BankTransferCheckoutContextQuery : IRequest<BankTransferCheckoutContextQueryResponse>
    {
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}