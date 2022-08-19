using FluentValidation;

namespace Mosaico.Application.Identity.Commands.DeactivateUser
{
    public class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
    {
        public DeactivateUserCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}