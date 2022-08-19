using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.Administration.GetAdminProjects
{
    public class GetAdminProjectsQueryValidator : AbstractValidator<GetAdminProjectsQuery>
    {
        public GetAdminProjectsQueryValidator()
        {
            RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
            RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}