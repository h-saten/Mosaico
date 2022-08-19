using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.ExportSubscribers
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class ExportSubscribersCommand : IRequest<ExportSubscribersCommandResponse>
    {
        public Guid ProjectId { get; set; }
    }
}