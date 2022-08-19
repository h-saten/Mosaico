using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.DisablePartner
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class DisabledPartnerCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid PartnerId { get; set; }
    }
}