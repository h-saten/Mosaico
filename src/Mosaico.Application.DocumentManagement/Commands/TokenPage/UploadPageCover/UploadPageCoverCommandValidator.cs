using System;
using System.Linq;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadPageCover
{
    public class UploadPageCoverCommandValidator : AbstractValidator<UploadPageCoverCommand>
    {
        public UploadPageCoverCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.PageId).Must(x => x != Guid.Empty);
            RuleFor(x => x.Language).Must(x => Mosaico.Base.Constants.Languages.All.Contains(x));
        }
    }
}