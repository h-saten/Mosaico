using System;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetAbout;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageFaq;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteFaq
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetPageFaqQuery), "{{PageId}}_*")]
    public class DeleteFaqCommand : IRequest
    {
        public Guid FaqId { get; set; }
        public Guid PageId { get; set; }
    }
}