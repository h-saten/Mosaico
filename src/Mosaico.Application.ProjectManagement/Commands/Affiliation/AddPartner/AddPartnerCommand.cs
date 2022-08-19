using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.AddPartner
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class AddPartnerCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public Guid ProjectId { get; set; }
    }
}