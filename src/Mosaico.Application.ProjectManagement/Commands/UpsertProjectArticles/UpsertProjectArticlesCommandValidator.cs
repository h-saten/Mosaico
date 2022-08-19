using System.Linq;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectArticles
{
    public class UpsertProjectArticlesCommandValidator : AbstractValidator<UpsertProjectArticlesCommand>
    {
        public UpsertProjectArticlesCommandValidator()
        {
            RuleFor(t => t.VisibleText).NotEmpty();
            RuleFor(t => t.Link).NotEmpty();
            RuleFor(t => t.CoverPicture).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}