using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployStage
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DeployStageCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid StageId { get; set; }
    }
}