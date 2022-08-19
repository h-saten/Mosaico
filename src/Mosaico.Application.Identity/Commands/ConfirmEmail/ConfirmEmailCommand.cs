using MediatR;
using Newtonsoft.Json;

namespace Mosaico.Application.Identity.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        
        [JsonIgnore]
        public string IP;
        [JsonIgnore]
        public string AgentInfo { get; set; }
    }
}