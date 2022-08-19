using System;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadPageCover
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UploadPageCoverCommand : UploadDocumentRequestBase
    {
        public override Guid GetEntityId()
        {
            return PageId;
        }
        public Guid PageId { get; set; }
    }
}