using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUserKycStatus
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetUserKycStatusQuery : IRequest<string>
    {
        public string UserId { get; set; }
    }
}