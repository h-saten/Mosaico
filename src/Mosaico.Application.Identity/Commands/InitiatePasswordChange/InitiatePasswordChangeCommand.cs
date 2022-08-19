using MediatR;

namespace Mosaico.Application.Identity.Commands.InitiatePasswordChange
{
    public class InitiatePasswordChangeCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}