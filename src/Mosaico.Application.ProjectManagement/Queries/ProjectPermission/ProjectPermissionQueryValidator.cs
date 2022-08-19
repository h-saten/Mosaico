using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectPermission
{
    public class ProjectPermissionQueryValidator : AbstractValidator<ProjectPermissionQuery>
    {
        public ProjectPermissionQueryValidator()
        {
            RuleFor(p => p.UniqueIdentifier).NotNull().NotEmpty();
        }
    }
}