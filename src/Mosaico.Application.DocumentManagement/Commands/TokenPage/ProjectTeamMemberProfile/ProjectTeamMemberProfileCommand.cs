using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.ProjectTeamMemberProfile
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDocuments)]
    public class ProjectTeamMemberProfileCommand : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public Guid ProjectId { get; set; }
    }
}
