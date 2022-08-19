using FluentValidation;

namespace Mosaico.Application.Identity.Commands.InitiatePasswordReset
{
    public class InitiatePasswordResetCommandValidator : AbstractValidator<InitiatePasswordResetCommand>
    {
        public InitiatePasswordResetCommandValidator()
        {
            RuleFor(c => c.Email).EmailAddress().NotEmpty();
        }
    }
}