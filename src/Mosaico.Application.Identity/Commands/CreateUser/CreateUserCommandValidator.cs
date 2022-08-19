using FluentValidation;

namespace Mosaico.Application.Identity.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Email).EmailAddress().WithErrorCode("EMAIL_INVALID");
            RuleFor(c => c.Email).NotNull().NotEmpty().WithErrorCode("EMAIL_REQUIRED");
            RuleFor(c => c.Password).MinimumLength(6).MaximumLength(100).WithErrorCode("PASSWORD_REQUIRED");
            RuleFor(c => c.ConfirmPassword).Must(((command, s) => s == command.Password)).WithErrorCode("CONFIRM_PASSWORD_NOT_MATCHING");
            RuleFor(c => c.Terms).Equal(true);
            RuleFor(c => c.NotForbiddenCitizenship).Equal(true);
        }
    }
}