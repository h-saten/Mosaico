using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetUserProjects
{
    public class GetUserProjectsQueryValidator : AbstractValidator<GetUserProjectsQuery>
    {
        public GetUserProjectsQueryValidator()
        {
            RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
            RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}