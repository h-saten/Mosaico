using FluentValidation;

namespace Mosaico.Application.Identity.Commands.ConfirmEmailChange
{
    public class ConfirmEmailChangeCommandValidator : AbstractValidator<ConfirmEmailChangeCommand>
    {
        public ConfirmEmailChangeCommandValidator()
        {
            RuleFor(c => c.UserId).NotNull().NotEmpty();
            RuleFor(c => c.Code).NotNull().NotEmpty();
            RuleFor(c => c.Email).NotNull().NotEmpty();
        }
    }
}