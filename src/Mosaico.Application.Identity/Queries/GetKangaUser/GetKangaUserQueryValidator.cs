using System;
using FluentValidation;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetKangaUser
{
    public class GetKangaUserQueryValidator : AbstractValidator<GetKangaUserQuery>
    {
        public GetKangaUserQueryValidator(ICurrentUserContext currentUserContext)
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.Id).NotEmpty().Must(q => Guid.TryParse(q, out var id)).WithErrorCode(Constants.ErrorCodes.InvalidId);
            });
        }
    }
}