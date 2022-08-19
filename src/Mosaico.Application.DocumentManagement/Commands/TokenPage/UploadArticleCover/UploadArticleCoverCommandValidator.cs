using System.Linq;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadArticleCover
{
    public class UploadArticlePhotoCommandValidator : AbstractValidator<UploadArticleCoverCommand>
    {
        public UploadArticlePhotoCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.Language).Must(x => Mosaico.Base.Constants.Languages.All.Contains(x));
        }
    }
}