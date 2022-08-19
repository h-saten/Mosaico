using FluentValidation;

namespace Mosaico.Application.Identity.Queries.GetUserProfilePermissions
{
    public class GetUserProfilePermissionsQueryValidator : AbstractValidator<GetUserProfilePermissionsQuery>
    {
        public GetUserProfilePermissionsQueryValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}