using FluentValidation;

namespace Mosaico.Application.Identity.Commands.VerifyPhoneNumber
{
    public class VerifyPhoneNumberCommandValidator : AbstractValidator<VerifyPhoneNumberCommand>
    {
        public VerifyPhoneNumberCommandValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.ConfirmationCode).NotEmpty();
        }
    }
}