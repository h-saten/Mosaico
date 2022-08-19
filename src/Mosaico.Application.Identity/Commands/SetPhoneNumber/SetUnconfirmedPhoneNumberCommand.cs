using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.SetPhoneNumber
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class SetUnconfirmedPhoneNumberCommand : IRequest
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
    }
}