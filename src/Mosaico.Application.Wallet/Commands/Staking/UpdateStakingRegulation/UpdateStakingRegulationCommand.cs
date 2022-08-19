using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Staking.UpdateStakingRegulation
{
    public class UpdateStakingRegulationCommand : IRequest
    {
        public Guid StakingPairId { get; set; }
        public string Regulation { get; set; }
        public string Language { get; set; }
    }
}