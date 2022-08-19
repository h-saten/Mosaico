using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.SDK.Identity.Pipelines;

namespace Mosaico.Application.ProjectManagement.Commands.HideArticle
{
    [Restricted(nameof(ArticleId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class HideArticleCommand : IRequest
    {
        public Guid ArticleId { get; set; }
    }
}