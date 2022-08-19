using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.VerifyUser
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class InitiateUserVerificationCommand : IRequest
    {
        public string UserId { get; set; }
    }
}