using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.ApproveProject
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class ApproveProjectCommand : IRequest
    {
        public Guid ProjectId { get; set; }
    }
}