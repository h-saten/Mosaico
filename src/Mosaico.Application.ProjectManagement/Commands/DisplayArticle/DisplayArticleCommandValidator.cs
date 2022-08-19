using FluentValidation;
using System;

namespace Mosaico.Application.ProjectManagement.Commands.DisplayArticle
{
    public class DisplayArticleCommandValidator : AbstractValidator<DisplayArticleCommand>
    {
        public DisplayArticleCommandValidator()
        {
            RuleFor(c => c.ArticleId).NotEmpty().Must(c => c != Guid.Empty);
        }
    }
}
