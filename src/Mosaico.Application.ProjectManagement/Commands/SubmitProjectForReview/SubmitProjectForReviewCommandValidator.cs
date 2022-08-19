using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.SubmitProjectForReview
{
    public class SubmitProjectForReviewCommandValidator : AbstractValidator<SubmitProjectForReviewCommand>
    {
        public SubmitProjectForReviewCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}