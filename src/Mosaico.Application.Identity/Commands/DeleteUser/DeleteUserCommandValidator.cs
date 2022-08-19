using FluentValidation;

namespace Mosaico.Application.Identity.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(c => c.Password).MinimumLength(6).MaximumLength(100).WithErrorCode("PASSWORD_REQUIRED");
        }
    }
}