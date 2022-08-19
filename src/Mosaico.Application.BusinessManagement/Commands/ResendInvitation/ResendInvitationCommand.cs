using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Commands.ResendInvitation
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class ResendInvitationCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
    }
}