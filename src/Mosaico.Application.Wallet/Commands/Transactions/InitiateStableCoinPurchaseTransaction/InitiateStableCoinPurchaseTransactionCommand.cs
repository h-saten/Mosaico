using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Transactions.InitiateStableCoinPurchaseTransaction
{
    public class InitiateStableCoinPurchaseTransactionCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
        public decimal TokenAmount { get; set; }
        public string PaymentProcessor { get; set; }
        public string WalletAddress { get; set; }
        public string Network { get; set; }
        public string PaymentCurrency { get; set; }
    }
}