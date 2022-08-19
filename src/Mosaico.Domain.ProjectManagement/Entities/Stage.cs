using System;
using System.Collections.Generic;
using System.Linq;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class Stage : EntityBase
    {
        public string Name { get; set; }
        public StageType Type { get; set; }
        public virtual Project Project { get; set; }
        public Guid ProjectId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal TokensSupply { get; set; }
        public decimal TokenPrice { get; set; }
        public decimal TokenPriceNativeCurrency { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
        public virtual StageStatus Status { get; set; }
        public Guid StatusId { get; set; }
        public Guid? VestingId { get; set; }
        public int Order { get; set; }
        public StageDeploymentStatus DeploymentStatus { get; set; }
        public DateTimeOffset? DeployedAt { get; set; }
        public bool AllowRedeployment { get; set; }
        public string AuthorizationCode { get; set; }
        public virtual List<ProjectInvestor> ProjectInvestors { get; set; } = new();
        public virtual List<StagePurchaseLimit> PurchaseLimits { get; set; } = new List<StagePurchaseLimit>();

        public void SetStatus(StageStatus status)
        {
            if (status != null)
            {
                Status = status;
                StatusId = status.Id;
            }
        }

        public void SetProject(Project project)
        {
            if (project != null)
            {
                Project = project;
                ProjectId = project.Id;
            }
        }

        public Tuple<decimal, decimal> GetPurchaseLimits(string paymentMethod = null)
        {
            var limit = PurchaseLimits?.FirstOrDefault(pl => paymentMethod != null && pl.PaymentMethod == paymentMethod);
            return limit != null ? new Tuple<decimal, decimal>(limit.MinimumPurchase, limit.MaximumPurchase) : new Tuple<decimal, decimal>(MinimumPurchase, MaximumPurchase);
        }
    }
}