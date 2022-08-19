using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetUserAffiliation
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    [Cache("{{UserId}}", ExpirationInMinutes = 2)]
    public class GetUserAffiliationQuery : IRequest<GetUserAffiliationQueryResponse>
    {
        public string UserId { get; set; }
    }
}