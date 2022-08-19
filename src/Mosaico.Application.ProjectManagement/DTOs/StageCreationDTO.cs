using System;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class StageCreationDTO
    {
        public string Name { get; set; }
        public StageType Type { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal TokensSupply { get; set; }
        public decimal TokenPrice { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
        public Guid? Id { get; set; }
    }
}