using FluentValidation;

namespace Mosaico.Application.Identity.Commands.SubscribeToNewsletter
{
    public class SubscribeToNewsletterValidator : AbstractValidator<SubscribeToNewsletterCommand>
    {
        public SubscribeToNewsletterValidator()
        {
            RuleFor(c => c.Email).NotEmpty();
        }
    }
}
