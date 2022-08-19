using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.Authenticator.GetAuthenticatorKey
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetAuthenticatorKeyQuery : IRequest<GetAuthenticatorKeyQueryResponse>
    {
        public string UserId { get; set; }   
    }
}