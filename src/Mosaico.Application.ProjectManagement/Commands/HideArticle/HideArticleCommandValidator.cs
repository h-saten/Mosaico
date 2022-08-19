using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.HideArticle
{
    public class HideArticleCommandValidator : AbstractValidator<HideArticleCommand>
    {
        public HideArticleCommandValidator()
        {
            RuleFor(c => c.ArticleId).NotEmpty().Must(c => c != Guid.Empty);
        }
    }
}