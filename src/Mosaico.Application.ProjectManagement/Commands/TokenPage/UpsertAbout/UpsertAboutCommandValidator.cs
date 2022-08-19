using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertAbout
{
    public class UpsertAboutCommandValidator : AbstractValidator<UpsertAboutCommand>
    {
        public UpsertAboutCommandValidator()
        {
            RuleFor(c => c.Content).NotEmpty();
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}