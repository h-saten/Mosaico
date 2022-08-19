using FluentValidation;

namespace Mosaico.Application.Identity.Commands.ConfirmChangePassword
{
    public class ConfirmPasswordChangeCommandValidator : AbstractValidator<ConfirmPasswordChangeCommand>
    {
        public ConfirmPasswordChangeCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.OldPassword).NotEmpty().WithErrorCode("OLD_PASSWORD_REQUIRED");
            RuleFor(c => c.NewPassword).MinimumLength(6).MaximumLength(50).WithErrorCode("PASSWORD_REQUIRED");
            RuleFor(c => c.ConfirmPassword).NotEmpty().Must((command, s) => command.NewPassword == command.ConfirmPassword).WithErrorCode("PASSWORD_DONT_MATCH");
        }
    }
}