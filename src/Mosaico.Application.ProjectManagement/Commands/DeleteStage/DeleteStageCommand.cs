using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteStage
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DeleteStageCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid StageId { get; set; }
    }
}