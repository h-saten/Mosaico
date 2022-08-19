using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.ApplyUserReference
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class ApplyUserReferenceCommand : IRequest
    {
        public string RefCode { get; set; }
        public string UserId { get; set; }
        public Guid? ProjectId { get; set; }
    }
}