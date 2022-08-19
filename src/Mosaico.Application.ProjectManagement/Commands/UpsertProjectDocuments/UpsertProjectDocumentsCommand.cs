using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetProjectDocument;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectDocuments
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectDocumentQuery), "{{ProjectId}}_{{Type}}_{{Language}}")]
    public class UpsertProjectDocumentsCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }

    }
}