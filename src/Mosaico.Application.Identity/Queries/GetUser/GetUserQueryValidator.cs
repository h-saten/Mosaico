using System;
using FluentValidation;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUser
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator(ICurrentUserContext currentUserContext)
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.Id).NotEmpty().Must(q => Guid.TryParse(q, out var id)).WithErrorCode(Constants.ErrorCodes.InvalidId);
            });
        }
    }
}