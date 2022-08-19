using Mosaico.Application.Identity.DTOs;

namespace Mosaico.Application.Identity.Commands.LoginUser
{
    public class LoginUserCommandResponse
    {
        public readonly LoginResult result;
        public LoginUserCommandResponse(LoginResult result = null)
        {
            this.result = result;
        }
    }
}