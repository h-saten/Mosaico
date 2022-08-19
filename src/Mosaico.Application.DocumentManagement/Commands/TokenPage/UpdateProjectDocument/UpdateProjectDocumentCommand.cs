using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UpdateProjectDocument
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDocuments)]
    public class UpdateProjectDocumentCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ProjectId { get; set; }
    }
}
