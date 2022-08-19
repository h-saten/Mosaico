using System;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPage;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.UpdatePage
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetTokenPageQuery), "{{PageId}}_*")]
    public class UpdatePageCommand : IRequest
    {
        public Guid PageId { get; set; }
        public UpdatePageDTO Page { get; set; }
    }
}