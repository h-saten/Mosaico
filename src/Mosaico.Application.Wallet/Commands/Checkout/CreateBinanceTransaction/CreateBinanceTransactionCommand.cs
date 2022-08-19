using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateBinanceTransaction
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class CreateBinanceTransactionCommand : IRequest<CreateBinanceTransactionCommandResponse>
    {
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string RefCode { get; set; }
        public decimal PayedAmount { get; set; }
        public string Currency { get; set; }
    }
}