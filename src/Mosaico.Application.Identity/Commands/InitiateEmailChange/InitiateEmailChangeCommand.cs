using MediatR;
using Mosaico.Authorization.Base;
using Newtonsoft.Json;

namespace Mosaico.Application.Identity.Commands.InitiateEmailChange
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class InitiateEmailChangeCommand : IRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}