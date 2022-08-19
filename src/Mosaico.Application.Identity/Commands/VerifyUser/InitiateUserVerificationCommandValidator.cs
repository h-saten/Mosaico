using FluentValidation;

namespace Mosaico.Application.Identity.Commands.VerifyUser
{
    public class InitiateUserVerificationCommandValidator : AbstractValidator<InitiateUserVerificationCommand>
    {
        public InitiateUserVerificationCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}