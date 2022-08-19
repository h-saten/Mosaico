using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.ClaimAirdrop
{
    public class ClaimAirdropCommandValidator : AbstractValidator<ClaimAirdropCommand>
    {
        public ClaimAirdropCommandValidator()
        {
            RuleFor(t => t.AirdropId).NotEmpty();
        }
    }
}