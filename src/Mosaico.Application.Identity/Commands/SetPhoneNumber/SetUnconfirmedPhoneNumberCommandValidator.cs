using FluentValidation;

namespace Mosaico.Application.Identity.Commands.SetPhoneNumber
{
    public class SetUnconfirmedPhoneNumberCommandValidator : AbstractValidator<SetUnconfirmedPhoneNumberCommand>
    {
        public SetUnconfirmedPhoneNumberCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.PhoneNumber).NotEmpty();
        }
    }
}