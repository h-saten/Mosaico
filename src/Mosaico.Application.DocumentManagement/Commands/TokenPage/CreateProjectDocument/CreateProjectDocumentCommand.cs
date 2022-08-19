using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.CreateProjectDocument
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDocuments)]
    public class CreateProjectDocumentCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public Guid ProjectId { get; set; }
        public bool IsMandatory { get; set; } = false;
    }
}
