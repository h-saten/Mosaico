using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.Authenticator.ResetAuthenticator
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class ResetAuthenticatorCommand : IRequest
    {
        public string UserId { get; set; }
    }
}