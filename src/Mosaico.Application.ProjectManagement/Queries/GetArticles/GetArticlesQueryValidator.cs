using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetArticles
{
    public class GetArticlesQueryValidator : AbstractValidator<GetArticlesQuery>
    {
        public GetArticlesQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
                RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
            });
        }
    }
}