using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.DistributeAirdrop
{
    public class DistributeAirdropCommandValidator : AbstractValidator<DistributeAirdropCommand>
    {
        public DistributeAirdropCommandValidator()
        {
            RuleFor(t => t.AirdropId).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}