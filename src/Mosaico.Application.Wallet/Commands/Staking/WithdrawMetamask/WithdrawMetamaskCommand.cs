using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Staking.WithdrawMetamask
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class WithdrawMetamaskCommand : IRequest
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string TransactionHash { get; set; }
        public string Wallet { get; set; }
    }
}