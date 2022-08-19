using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateInvitation
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class UpdateTeamMemberCommand : IRequest<Guid>
    {
        [JsonIgnore] 
        public Guid Id { get; set; }

        [JsonIgnore] 
        public Guid CompanyId { get; set; }

        public string RoleName { get; set; }
    }
}