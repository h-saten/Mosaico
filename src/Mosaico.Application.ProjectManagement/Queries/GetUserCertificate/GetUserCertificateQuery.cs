using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.GetUserCertificate
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetUserCertificateQuery : IRequest<FileContentResult>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
        public string Language { get; set; }
    }
}