using System;
using System.Linq;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadArticlePhoto
{
    public class UploadArticlePhotoCommandValidator : AbstractValidator<UploadArticlePhotoCommand>
    {
        public UploadArticlePhotoCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.ArticleId).Must(x => x != Guid.Empty);
            RuleFor(x => x.Language).Must(x => Mosaico.Base.Constants.Languages.All.Contains(x));
        }
    }
}