using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
    {
        public AcceptInvitationCommandValidator()
        {
            RuleFor(c => c.Code).NotNull().NotEmpty();
        }
    }
}