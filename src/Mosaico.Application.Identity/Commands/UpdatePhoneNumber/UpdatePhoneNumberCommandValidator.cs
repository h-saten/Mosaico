using FluentValidation;

namespace Mosaico.Application.Identity.Commands.UpdatePhoneNumber
{
    public class UpdatePhoneNumberCommandValidator : AbstractValidator<UpdatePhoneNumberCommand>
    {
        public UpdatePhoneNumberCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Code).NotEmpty();
            RuleFor(c => c.PhoneNumber).NotEmpty();
        }
    }
}