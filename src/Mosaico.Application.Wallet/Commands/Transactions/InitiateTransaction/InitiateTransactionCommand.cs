using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Transactions.InitiateTransaction
{
    public class InitiateTransactionCommand : IRequest<Guid>
    {
        public string PaymentProcessor { get; set; }
        public string WalletAddress { get; set; }
        
        public string Network { get; set; }
        public decimal? TokenAmount { get; set; }
        public Guid ProjectId { get; set; }
        public string PaymentCurrency { get; set; }
    }
}