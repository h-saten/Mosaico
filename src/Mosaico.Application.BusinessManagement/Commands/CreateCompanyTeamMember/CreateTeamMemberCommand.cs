using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Commands.CreateCompanyTeamMember
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class CreateTeamMemberCommand : IRequest<Guid>
    {
        public Guid CompanyId { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}