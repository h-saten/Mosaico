using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteProjectTeamMember
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DeleteProjectTeamMemberCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
    }
}
