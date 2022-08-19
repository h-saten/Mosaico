using FluentValidation;
using Mosaico.Application.ProjectManagement.Validators;

namespace Mosaico.Application.ProjectManagement.Commands.UpdatePage
{
    public class UpdatePageCommandValidator : AbstractValidator<UpdatePageCommand>
    {
        public UpdatePageCommandValidator()
        {
            RuleFor(t => t.PageId).NotEmpty();
            RuleFor(t => t.Page).NotNull();
            RuleFor(t => t.Page).SetValidator(new UpdatePageValidator());
        }
    }
}