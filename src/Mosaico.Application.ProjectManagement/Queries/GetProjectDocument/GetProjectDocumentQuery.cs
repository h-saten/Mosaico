using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectDocument
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    // [Cache("{{ProjectId}}_{{Type}}_{{Language}}")]
    public class GetProjectDocumentQuery : IRequest<DocumentContentDTO>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        
        [JsonIgnore]
        public string Type { get; set; }
        public string Language { get; set; }
    }
}