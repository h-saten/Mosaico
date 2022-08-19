using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.ConfirmEmailChange
{
    public class ConfirmEmailChangeCommand : IRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }
}