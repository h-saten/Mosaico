using System;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateBankTransferTransaction
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class CreateBankTransferTransactionCommand : IRequest<ProjectBankPaymentDetailsDTO>
    {
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Currency { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal FiatAmount { get; set; }
        public string RefCode { get; set; }
    }
}