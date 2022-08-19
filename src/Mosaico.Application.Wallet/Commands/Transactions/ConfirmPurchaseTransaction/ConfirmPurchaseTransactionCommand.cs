using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Transactions.ConfirmPurchaseTransaction
{
    public class ConfirmPurchaseTransactionCommand : IRequest
    {
        public decimal PayedAmount { get; set; }
        public string Currency { get; set; }
        public Guid? TransactionId { get; set; }
    }
}