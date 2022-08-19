using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.DisplayArticle
{
    [Restricted(nameof(ArticleId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DisplayArticleCommand : IRequest
    {
        public Guid ArticleId { get; set; }
    }
}
