using System;
using FluentValidation;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUserPermissions
{
    public class GetUserPermissionsValidator : AbstractValidator<GetUserPermissionsQuery>
    {
        public GetUserPermissionsValidator()
        {
            RuleFor(q => q.Id).NotEmpty().Must(q => Guid.TryParse(q, out var id)).WithErrorCode(Constants.ErrorCodes.InvalidId);
        }
    }
}