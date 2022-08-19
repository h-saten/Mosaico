using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Models.CertificateGenerator;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertCertificateConfiguration
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpsertCertificateConfigurationCommand : IRequest
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public LogoBlock Logo { get; set; }
        public BaseTextBlock Name { get; set; }
        public TokensAmountBaseBlock Tokens { get; set; }
        public BaseTextBlock Date { get; set; }
        public BaseTextBlock Code { get; set; }
        public bool SendingEnabled { get; set; }
    }
}