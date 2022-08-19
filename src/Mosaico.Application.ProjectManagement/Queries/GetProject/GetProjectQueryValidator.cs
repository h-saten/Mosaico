using FluentValidation;
using Mosaico.Application.ProjectManagement.Queries.GetProjects;

namespace Mosaico.Application.ProjectManagement.Queries.GetProject
{
    public class GetProjectQueryValidator : AbstractValidator<GetProjectQuery>
    {
        public GetProjectQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(d => d.UniqueIdentifier).NotEmpty();
            });
        }
    }
}