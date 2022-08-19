using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.CompanyPermissions
{
    public class CompanyPermissionQueryValidator : AbstractValidator<CompanyPermissionQuery>
    {
        public CompanyPermissionQueryValidator()
        {
            RuleFor(p => p.UniqueIdentifier).NotNull().NotEmpty();
        }
    }
}