using FluentValidation;

namespace Mosaico.Application.Core.Commands.SendContactForm
{
    public class SendContactFormCommandValidator : AbstractValidator<SendContactFormCommand>
    {
        public SendContactFormCommandValidator()
        {
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.EmailAddress).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}
