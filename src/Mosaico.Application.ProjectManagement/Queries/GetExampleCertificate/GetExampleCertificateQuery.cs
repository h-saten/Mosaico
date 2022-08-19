using System;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetExampleCertificate
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetExampleCertificateQuery : IRequest<FileContentResult>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Language { get; set; }
    }
}