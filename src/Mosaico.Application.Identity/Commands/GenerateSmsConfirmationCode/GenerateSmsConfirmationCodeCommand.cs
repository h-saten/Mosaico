using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.GenerateSmsConfirmationCode
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GenerateSmsConfirmationCodeCommand : IRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}