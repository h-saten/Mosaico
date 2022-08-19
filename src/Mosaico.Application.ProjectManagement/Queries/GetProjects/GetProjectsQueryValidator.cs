using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjects
{
    public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
    {
        public GetProjectsQueryValidator()
        {
            RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
            RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}