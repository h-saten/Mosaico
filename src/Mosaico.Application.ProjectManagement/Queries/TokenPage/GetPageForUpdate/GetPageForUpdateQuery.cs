using System;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageForUpdate
{
    [Restricted(nameof(Id), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetPageForUpdateQuery : IRequest<UpdatePageDTO>
    {
        public Guid Id { get; set; }
    }
}