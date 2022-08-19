using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.VerifyPhoneNumber
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class VerifyPhoneNumberCommand : IRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        
        public string PhoneNumber { get; set; }
        public string ConfirmationCode { get; set; }
    }
}