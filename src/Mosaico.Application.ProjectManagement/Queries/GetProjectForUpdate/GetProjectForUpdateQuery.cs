using System;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectForUpdate
{
    [Restricted(nameof(Id), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetProjectForUpdateQuery : IRequest<UpdateProjectDTO>
    {
        public Guid Id { get; set; }
    }
}