using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateTransactionFee
{
    //restriction is in handler
    public class UpdateTransactionFeeCommand : IRequest
    {
        public Guid TransactionId { get; set; }
        public int FeePercentage { get; set; }
    }
}