using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.DeletePageCover
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DeletePageCoverCommand : IRequest
    {
        public Guid PageId { get; set; }
        public string Language { get; set; }
    }
}