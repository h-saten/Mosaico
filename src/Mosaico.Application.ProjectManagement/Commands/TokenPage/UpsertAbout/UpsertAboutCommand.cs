using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetAbout;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertAbout
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetAboutQuery), "{{PageId}}_{{Language}}")]
    public class UpsertAboutCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid PageId { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
    }
}