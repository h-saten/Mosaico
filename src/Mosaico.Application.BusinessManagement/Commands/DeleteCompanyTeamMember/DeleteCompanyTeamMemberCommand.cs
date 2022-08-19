using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Commands.DeleteCompanyTeamMember
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class DeleteCompanyTeamMemberCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
    }
}