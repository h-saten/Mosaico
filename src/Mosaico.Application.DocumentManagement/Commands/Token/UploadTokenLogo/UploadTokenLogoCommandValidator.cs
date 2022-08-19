using System;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.Token.UploadTokenLogo
{
    public class UploadTokenLogoCommandValidator : AbstractValidator<UploadTokenLogoCommand>
    {
        public UploadTokenLogoCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.TokenId).Must(x => x != Guid.Empty);
        }
    }
}