using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteArticle
{
    public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
    {
        public DeleteArticleCommandValidator()
        {
            RuleFor(c => c.ArticleId).NotEmpty().Must(c => c != Guid.Empty);
        }
    }
}