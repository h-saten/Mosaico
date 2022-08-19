using FluentValidation;

namespace Mosaico.Application.Identity.Commands.GenerateSmsConfirmationCode
{
    public class GenerateSmsConfirmationCodeCommandValidator : AbstractValidator<GenerateSmsConfirmationCodeCommand>
    {
        public GenerateSmsConfirmationCodeCommandValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}