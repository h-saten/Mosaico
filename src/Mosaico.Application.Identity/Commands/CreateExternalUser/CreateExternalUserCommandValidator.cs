using FluentValidation;

namespace Mosaico.Application.Identity.Commands.CreateExternalUser
{
    public class CreateExternalUserCommandValidator : AbstractValidator<CreateExternalUserCommand>
    {
        public CreateExternalUserCommandValidator()
        {
            RuleFor(c => c.Email).EmailAddress().WithErrorCode("EMAIL_INVALID");
            RuleFor(c => c.Email).NotNull().NotEmpty().WithErrorCode("EMAIL_REQUIRED");
        }
    }
}