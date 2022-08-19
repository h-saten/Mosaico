using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectTeamMember
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class CreateUpdateProjectTeamMemberCommand : IRequest<Guid>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public int Order { get; set; }
        public string PhotoUrl { get; set; }

        [JsonIgnore]
        public Guid PageId { get; set; }
    }
}
