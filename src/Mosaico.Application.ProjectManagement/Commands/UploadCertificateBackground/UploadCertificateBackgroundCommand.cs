using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.UploadCertificateBackground
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UploadCertificateBackgroundCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
        public string OriginalFileName { get; set; }
        public byte[] Content { get; set; }
    }
}