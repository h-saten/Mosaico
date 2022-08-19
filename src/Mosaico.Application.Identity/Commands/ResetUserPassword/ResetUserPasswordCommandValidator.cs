using FluentValidation;

namespace Mosaico.Application.Identity.Commands.ResetUserPassword
{
    public class ResetUserPasswordCommandValidator : AbstractValidator<ResetUserPasswordCommand>
    {
        public ResetUserPasswordCommandValidator()
        {
            RuleFor(c => c.Email).NotEmpty();
            RuleFor(c => c.Code).NotEmpty();
            RuleFor(c => c.Password).MinimumLength(6).MaximumLength(100).WithErrorCode("PASSWORD_REQUIRED");
            RuleFor(c => c.ConfirmPassword).MinimumLength(6).MaximumLength(100).WithErrorCode("PASSWORD_REQUIRED");
        }
    }
}