using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployCrowdsale
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DeployCrowdsaleCommand : IRequest
    {
        public Guid ProjectId { get; set; }
    }
}