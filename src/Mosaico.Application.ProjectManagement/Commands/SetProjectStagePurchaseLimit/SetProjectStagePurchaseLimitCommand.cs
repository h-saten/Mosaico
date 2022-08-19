using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.SetProjectStagePurchaseLimit
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class SetProjectStagePurchaseLimitCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid StageId { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
        public string PaymentMethod { get; set; }
    }
}