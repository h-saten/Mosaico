using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Staking.StakeMetamask
{
    public class StakeMetamaskCommand : IRequest
    {
        public Guid StakingPairId { get; set; }
        public decimal Balance { get; set; }
        public int Days { get; set; }
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
    }
}