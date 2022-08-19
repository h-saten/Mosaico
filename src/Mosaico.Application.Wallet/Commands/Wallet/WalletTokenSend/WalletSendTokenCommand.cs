using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Wallet.WalletTokenSend
{
    [ShouldCompleteEvaluation]
    public class WalletSendTokenCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public string WalletAddress { get; set; }
        public string Network { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
    }
}