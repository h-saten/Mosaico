using MediatR;
using System;
using Mosaico.Authorization.Base;
using Mosaico.SDK.Identity.Pipelines;

namespace Mosaico.Application.DocumentManagement.Commands.RemoveDocumentContent
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDocuments)]
    public class RemoveProjectDocumentCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid DocumentId { get; set; }
        public string Language { get; set; }
    }
}
