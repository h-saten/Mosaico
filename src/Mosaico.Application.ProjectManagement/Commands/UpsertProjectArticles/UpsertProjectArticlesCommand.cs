using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectArticles
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpsertProjectArticlesCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }

        public Guid ArticleId { get; set; }
        public Guid CoverId { get; set; }
        public Guid PhotoId { get; set; }
        public string VisibleText { get; set; }
        public string AuthorPhoto { get; set; }
        public string CoverPicture { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
    }
}