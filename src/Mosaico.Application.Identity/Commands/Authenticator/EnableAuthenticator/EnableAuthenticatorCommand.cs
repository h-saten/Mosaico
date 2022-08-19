using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.Authenticator.EnableAuthenticator
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class EnableAuthenticatorCommand : IRequest<EnableAuthenticatorCommandResponse>
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}