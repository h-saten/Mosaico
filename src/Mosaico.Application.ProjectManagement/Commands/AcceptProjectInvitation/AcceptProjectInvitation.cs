using System;
using MediatR;

namespace Mosaico.Application.ProjectManagement.Commands.AcceptProjectInvitation
{
    public class AcceptProjectInvitationCommand : IRequest<Guid>
    {
        public string AuthorizationCode { get; set; }
    }
}