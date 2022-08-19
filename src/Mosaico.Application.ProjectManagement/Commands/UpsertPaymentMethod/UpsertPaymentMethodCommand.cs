using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertPaymentMethod
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpsertPaymentMethodCommand : IRequest
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string PaymentMethodKey { get; set; }
        public bool IsEnabled { get; set; }
    }
}