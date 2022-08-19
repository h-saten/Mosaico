using System;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.User.UploadUserPhoto
{
    public class UploadUserPhotoCommandValidator : AbstractValidator<UploadUserPhotoCommand>
    {
        public UploadUserPhotoCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.UserId).Must(x => x != Guid.Empty);
        }
    }
}