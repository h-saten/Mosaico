using FluentValidation;

namespace Mosaico.Application.Identity.Queries.HasPermission
{
    public class HasPermissionQueryValidator : AbstractValidator<HasPermissionQuery>
    {
        public HasPermissionQueryValidator()
        {
            RuleFor(q => q.UserId).NotEmpty();
            RuleFor(q => q.Key).NotNull().NotEmpty();
        }
    }
}