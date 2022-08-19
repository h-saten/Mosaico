using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.ResendInvitation
{
    public class ResendInvitationCommandValidator : AbstractValidator<ResendInvitationCommand>
    {
        public ResendInvitationCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}