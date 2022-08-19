using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Staking.Claim
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class ClaimCommand : IRequest
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
    }
}