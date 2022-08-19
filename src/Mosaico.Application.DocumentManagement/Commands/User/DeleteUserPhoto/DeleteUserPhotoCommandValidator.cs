using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.User.DeleteUserPhoto
{
    public class DeleteUserPhotoCommandValidator : AbstractValidator<DeleteUserPhotoCommand>
    {
        public DeleteUserPhotoCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }
}