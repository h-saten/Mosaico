using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetCertificateConfiguration
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    // [Cache("{{ProjectId}}_{{Language}}")]
    public class GetCertificateConfigurationQuery : IRequest<GetCertificateConfigurationResponse>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
    }
}