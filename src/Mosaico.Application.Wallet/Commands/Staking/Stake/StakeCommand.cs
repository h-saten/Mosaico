using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Staking.Stake
{
    public class StakeCommand : IRequest
    {
        public Guid StakingPairId { get; set; }
        public decimal Balance { get; set; }
        public int Days { get; set; }
    }
}