using FluentValidation;

namespace Mosaico.Application.Identity.Commands.InitiateEmailChange
{
    public class InitiateEmailChangeCommandValidator : AbstractValidator<InitiateEmailChangeCommand>
    {
        public InitiateEmailChangeCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().NotNull();
            RuleFor(c => c.Password).NotEmpty().NotNull();
        }
    }
}