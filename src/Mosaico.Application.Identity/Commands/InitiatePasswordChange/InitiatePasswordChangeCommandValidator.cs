using FluentValidation;

namespace Mosaico.Application.Identity.Commands.InitiatePasswordChange
{
    public class InitiatePasswordChangeCommandValidator : AbstractValidator<InitiatePasswordChangeCommand>
    {
        public InitiatePasswordChangeCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.OldPassword).NotEmpty().WithErrorCode("OLD_PASSWORD_REQUIRED");
            RuleFor(c => c.NewPassword).MinimumLength(6).MaximumLength(50).WithErrorCode("PASSWORD_REQUIRED");
            RuleFor(c => c.ConfirmPassword).Must((command, s) => command.NewPassword == s).WithErrorCode("PASSWORD_DONT_MATCH");
        }
    }
}