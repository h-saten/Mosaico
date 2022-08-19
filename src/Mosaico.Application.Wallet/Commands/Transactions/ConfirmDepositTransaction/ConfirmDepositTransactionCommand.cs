using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Transactions.ConfirmDepositTransaction
{
    public class ConfirmDepositTransactionCommand : IRequest
    {
        public Guid? TransactionId { get; set; }
        public string Currency { get; set; }
        public decimal PayedAmount { get; set; }
    }
}