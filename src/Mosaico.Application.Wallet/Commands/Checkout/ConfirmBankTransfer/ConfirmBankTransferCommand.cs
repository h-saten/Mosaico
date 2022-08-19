using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Checkout.ConfirmBankTransfer
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class ConfirmBankTransferCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid TransactionId { get; set; }
    }
}