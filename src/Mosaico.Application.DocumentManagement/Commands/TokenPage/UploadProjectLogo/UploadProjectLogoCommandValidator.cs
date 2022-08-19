using System;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadProjectLogo
{
    public class UploadProjectLogoCommandValidator : AbstractValidator<UploadProjectLogoCommand>
    {
        public UploadProjectLogoCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.ProjectId).Must(x => x != Guid.Empty);
        }
    }
}