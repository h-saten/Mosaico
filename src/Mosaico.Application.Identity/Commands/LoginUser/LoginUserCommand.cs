using System.Text.Json.Serialization;
using MediatR;

namespace Mosaico.Application.Identity.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserCommandResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public string ReturnUrl { get; set; }
        public string AuthorizeDeviceCode { get; set; }
        
        [JsonIgnore]
        public string AfterLoginRedirectUrl { get; set; }
        [JsonIgnore]
        public string IP;
        [JsonIgnore]
        public string AgentInfo { get; set; }
    }
}