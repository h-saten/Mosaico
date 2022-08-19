using FluentValidation;

namespace Mosaico.Application.Identity.Commands.Authenticator.EnableAuthenticator
{
    public class EnableAuthenticatorCommandValidator : AbstractValidator<EnableAuthenticatorCommand>
    {
        public EnableAuthenticatorCommandValidator()
        {
            RuleFor(t => t.Code).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}