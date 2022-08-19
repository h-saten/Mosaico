using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectScore
{
    public class ProjectScoreQueryValidator : AbstractValidator<ProjectScoreQuery>
    {
        public ProjectScoreQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}