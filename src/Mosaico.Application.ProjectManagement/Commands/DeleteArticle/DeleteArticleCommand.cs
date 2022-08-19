using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.SDK.Identity.Pipelines;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteArticle
{
    [Restricted(nameof(ArticleId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DeleteArticleCommand : IRequest
    {
        public Guid ArticleId { get; set; }
    }
}