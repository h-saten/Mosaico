using System;
using MediatR;

namespace Mosaico.Application.BusinessManagement.Commands.AcceptInvitation
{
    public class AcceptInvitationCommand : IRequest<Guid>
    {
        public string Code { get; set; }
    }
}