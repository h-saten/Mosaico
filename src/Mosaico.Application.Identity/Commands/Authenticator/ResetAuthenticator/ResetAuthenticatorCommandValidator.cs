using FluentValidation;

namespace Mosaico.Application.Identity.Commands.Authenticator.ResetAuthenticator
{
    public class ResetAuthenticatorCommandValidator : AbstractValidator<ResetAuthenticatorCommand>
    {
        public ResetAuthenticatorCommandValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}