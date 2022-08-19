using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPageReviews
{
    public class GetTokenPageReviewQueryValidator : AbstractValidator<GetTokenPageReviewsQuery>
    {
        public GetTokenPageReviewQueryValidator()
        {
            RuleFor(t => t.PageId).NotEmpty();
        }
    }
}