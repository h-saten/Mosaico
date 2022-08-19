using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Staking.ClaimMetamask
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class ClaimMetamaskCommand : IRequest
    {
        public Guid StakingPairId { get; set; }
        public string UserId { get; set; }
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
        public decimal Amount { get; set; }
    }
}