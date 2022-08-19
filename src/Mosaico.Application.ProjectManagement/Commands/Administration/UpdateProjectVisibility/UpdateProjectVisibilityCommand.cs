using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectVisibility
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class UpdateProjectVisibilityCommand : IRequest
    {
        public Guid Id { get; set; }
        public bool Visibility { get; set; }
    }
}