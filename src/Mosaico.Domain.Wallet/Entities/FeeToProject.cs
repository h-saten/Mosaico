using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class FeeToProject : EntityBase
    {
        public Guid ProjectId { get; set; }
        public Guid? StageId { get; set; }
        public decimal FeePercentage { get; set; }
    }
}