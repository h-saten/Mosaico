using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.ConfirmChangePassword
{
    public class ConfirmPasswordChangeCommand : IRequest
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}