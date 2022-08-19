using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.DeleteProjectDocument
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDocuments)]
    public class DeleteProjectDocumentCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
    }
}
