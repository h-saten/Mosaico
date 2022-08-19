using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.AddAirdropParticipants
{
    public class AddAirdropParticipantsCommandValidator : AbstractValidator<AddAirdropParticipantsCommand>
    {
        public AddAirdropParticipantsCommandValidator()
        {
            RuleFor(t => t.Emails).NotEmpty();
            RuleFor(t => t.AirdropId).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}