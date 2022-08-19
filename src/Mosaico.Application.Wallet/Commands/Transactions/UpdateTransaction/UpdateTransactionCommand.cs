using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateTransaction
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class UpdateTransactionCommand : IRequest
    {
        public Guid TransactionId { get; set; }
        public decimal? PayedAmount { get; set; }
    }
}