using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Vesting.RedeployVesting
{
    //Restriction is in command handler
    public class RedeployVestingCommand : IRequest
    {
        public Guid VestingId { get; set; }
    }
}