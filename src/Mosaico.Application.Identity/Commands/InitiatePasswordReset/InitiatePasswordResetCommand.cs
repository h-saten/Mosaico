using MediatR;

namespace Mosaico.Application.Identity.Commands.InitiatePasswordReset
{
    public class InitiatePasswordResetCommand : IRequest
    {
        public string Email { get; set; }
    }
}