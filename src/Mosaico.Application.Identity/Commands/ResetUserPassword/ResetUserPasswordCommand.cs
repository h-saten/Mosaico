using MediatR;

namespace Mosaico.Application.Identity.Commands.ResetUserPassword
{
    public class ResetUserPasswordCommand : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}