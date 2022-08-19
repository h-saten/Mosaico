using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertPaymentCurrency
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpsertPaymentCurrencyCommand : IRequest
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string PaymentCurrencyAddress { get; set; }
        public bool IsEnabled { get; set; }
    }
}