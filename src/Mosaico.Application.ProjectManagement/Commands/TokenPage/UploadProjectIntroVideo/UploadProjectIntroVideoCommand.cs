using MediatR;
using Mosaico.Authorization.Base;
using Newtonsoft.Json;
using System;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UploadProjectIntroVideo
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UploadProjectIntroVideoCommand: IRequest<Guid>
    {
        [JsonIgnore]
        public Guid PageId { get; set; }
        public string VideoExternalLink { get; set; }
        public bool ShowLocalVideo { get; set; }
        public byte[] Content { get; set; }
    }
}
