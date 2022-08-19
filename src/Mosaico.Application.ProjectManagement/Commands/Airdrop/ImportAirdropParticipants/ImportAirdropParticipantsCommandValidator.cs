using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.ImportAirdropParticipants
{
    public class ImportAirdropParticipantsCommandValidator : AbstractValidator<ImportAirdropParticipantsCommand>
    {
        public ImportAirdropParticipantsCommandValidator()
        {
            RuleFor(t => t.File).NotNull().NotEmpty();
        }
    }
}