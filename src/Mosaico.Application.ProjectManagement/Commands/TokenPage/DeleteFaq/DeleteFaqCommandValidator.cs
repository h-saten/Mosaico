using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteFaq
{
    public class DeleteFaqCommandValidator : AbstractValidator<DeleteFaqCommand>
    {
        public DeleteFaqCommandValidator()
        {
            RuleFor(c => c.FaqId).NotEmpty();
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}