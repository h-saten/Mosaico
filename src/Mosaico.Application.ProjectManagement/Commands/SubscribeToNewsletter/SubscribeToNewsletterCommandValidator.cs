using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.SubscribeToNewsletter
{
    public class SubscribeToNewsletterCommandValidator : AbstractValidator<SubscribeToNewsletterCommand>
    {
        public SubscribeToNewsletterCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(c => c.Email).NotNull().When(c => string.IsNullOrWhiteSpace(c.UserId));
        }
    }
}