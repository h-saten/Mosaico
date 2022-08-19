using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertPageReview
{
    public class UpsertPageReviewCommandValidator : AbstractValidator<UpsertPageReviewCommand>
    {
        public UpsertPageReviewCommandValidator()
        {
            RuleFor(t => t.Link).NotEmpty();
            RuleFor(t => t.PageId).NotEmpty();
        }
    }
}