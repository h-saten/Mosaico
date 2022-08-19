using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Wallet.WalletCurrencySend
{
    [ShouldCompleteEvaluation]
    public class WalletSendCurrencyCommand : IRequest
    {
        public Guid PaymentCurrencyId { get; set; }
        public string WalletAddress { get; set; }
        public string Network { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
    }
}