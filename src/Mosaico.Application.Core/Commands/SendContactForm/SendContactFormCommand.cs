using MediatR;

namespace Mosaico.Application.Core.Commands.SendContactForm
{
    public class SendContactFormCommand : IRequest
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
    }
}