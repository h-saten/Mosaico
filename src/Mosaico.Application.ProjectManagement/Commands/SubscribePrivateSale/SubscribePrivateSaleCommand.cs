using MediatR;

namespace Mosaico.Application.ProjectManagement.Commands.SubscribePrivateSale
{
    public class SubscribePrivateSaleCommand : IRequest<SubscribePrivateSaleCommandResponse>
    {
        public string AuthorizationCode { get; set; }
    }
}