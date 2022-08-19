using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.ProjectPartnerProfile
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDocuments)]
    public class ProjectPartnerProfileCommand : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public Guid ProjectId { get; set; }
    }
}
