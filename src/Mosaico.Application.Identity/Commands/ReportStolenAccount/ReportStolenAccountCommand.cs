using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.ReportStolenAccount
{
    public class ReportStolenAccountCommand : IRequest
    {
        public string Id { get; set; }
        public string Code { get; set; }

    }
}