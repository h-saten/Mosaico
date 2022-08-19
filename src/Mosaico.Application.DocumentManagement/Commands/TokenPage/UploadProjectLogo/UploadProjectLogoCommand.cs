using System;
using MediatR;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadProjectLogo
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UploadProjectLogoCommand : UploadDocumentRequestBase
    {
        public override Guid GetEntityId()
        {
            return ProjectId;
        }

        public Guid ProjectId { get; set; }
    }
}