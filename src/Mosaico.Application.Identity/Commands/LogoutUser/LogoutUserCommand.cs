using MediatR;

namespace Mosaico.Application.Identity.Commands.LogoutUser
{
    public class LogoutUserCommand : IRequest<LogoutUserCommandResponse>
    {
        public string LogoutId { get; set; }
        public string ReturnUrl{ get; set; }
    }
}