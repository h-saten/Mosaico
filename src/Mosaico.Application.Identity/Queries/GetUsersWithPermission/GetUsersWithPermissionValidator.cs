using FluentValidation;

namespace Mosaico.Application.Identity.Queries.GetUsersWithPermission
{
    public class GetUsersWithPermissionValidator : AbstractValidator<GetUsersWithPermissionQuery>
    {
        public GetUsersWithPermissionValidator()
        {
            RuleFor(p => p.Key).NotEmpty();
        }
    }
}