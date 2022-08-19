using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UnsubscribeFromNewsletter
{
    public class UnsubscribeFromNewsletterCommandValidator : AbstractValidator<UnsubscribeFromNewsletterCommand>
    {
        public UnsubscribeFromNewsletterCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Email).NotEmpty().When(command => string.IsNullOrWhiteSpace(command.UserId));
            RuleFor(t => t.UserId).NotEmpty().When(command => string.IsNullOrWhiteSpace(command.Email));
        }
    }
}