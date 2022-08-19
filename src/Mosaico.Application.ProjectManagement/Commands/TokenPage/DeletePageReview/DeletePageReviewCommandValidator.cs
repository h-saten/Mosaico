using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeletePageReview
{
    public class DeletePageReviewCommandValidator : AbstractValidator<DeletePageReviewCommand>
    {
        public DeletePageReviewCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.PageId).NotEmpty();
        }
    }
}