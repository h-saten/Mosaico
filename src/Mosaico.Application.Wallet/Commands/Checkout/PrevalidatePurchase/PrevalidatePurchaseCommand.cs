using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Checkout.PrevalidatePurchase
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class PrevalidatePurchaseCommand : IRequest<PrevalidatePurchaseCommandResponse>
    {
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal TokenAmount { get; set; }
        public string Currency { get; set; }
        public decimal PayedAmount { get; set; }
        public string PaymentMethod { get; set; }
    }
}