using MediatR;
using Mosaico.Authorization.Base;
using Newtonsoft.Json;
using System;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpdateProjectIntroVideoUrl
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpdateProjectIntroVideoUrlCommand: IRequest<Guid>
    {
        [JsonIgnore]
        public Guid PageId { get; set; }
        public bool ShowLocalVideo { get; set; }
        public string VideoUrl { get; set; }
    }
}
