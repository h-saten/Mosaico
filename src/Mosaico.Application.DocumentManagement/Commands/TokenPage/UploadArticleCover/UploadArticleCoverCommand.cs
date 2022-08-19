using System;
using MediatR;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadArticleCover
{
    [Restricted(nameof(ArticleId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UploadArticleCoverCommand : IRequest<UpdateArticleDTO>
    {
        public string Language { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public Guid ArticleId { get; set; }
    }
}