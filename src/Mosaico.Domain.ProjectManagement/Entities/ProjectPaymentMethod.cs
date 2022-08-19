using System;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class ProjectPaymentMethod
    {
        public virtual Project Project { get; set; }
        public Guid ProjectId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}