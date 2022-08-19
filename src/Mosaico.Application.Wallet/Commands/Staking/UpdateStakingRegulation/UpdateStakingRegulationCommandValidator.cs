using System.Linq;
using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.UpdateStakingRegulation
{
    public class UpdateStakingRegulationCommandValidator : AbstractValidator<UpdateStakingRegulationCommand>
    {
        public UpdateStakingRegulationCommandValidator()
        {
            RuleFor(t => t.Language).Must(l => Base.Constants.Languages.All.Contains(l));
            RuleFor(t => t.StakingPairId).NotEmpty();
        }
    }
}