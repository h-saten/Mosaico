using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetProjectDocument;
using Mosaico.Application.ProjectManagement.Queries.GetProjectDocuments;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.UploadProjectDocuments
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UploadProjectDocumentsCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public byte[] Content { get; set; }
    }
}