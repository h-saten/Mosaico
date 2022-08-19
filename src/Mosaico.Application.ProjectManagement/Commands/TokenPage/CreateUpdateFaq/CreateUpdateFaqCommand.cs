using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageFaq;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateFaq
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetPageFaqQuery), "{{PageId}}_{{Language}}")]
    public class CreateUpdateFaqCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid PageId { get; set; }
        [JsonIgnore]
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsHidden { get; set; }
        public int Order { get; set; }
        public string Language { get; set; }
    }
}