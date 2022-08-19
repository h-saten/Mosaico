using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.StopAirdrop
{
    public class StopAirdropCommandValidator : AbstractValidator<StopAirdropCommand>
    {
        public StopAirdropCommandValidator()
        {
            RuleFor(t => t.AirdropId).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}