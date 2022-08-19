using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class PaymentMethod : EntityBase
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public virtual List<Project> Projects { get; set; } = new();
        public virtual List<ProjectPaymentMethod> ProjectPaymentMethods { get; set; } = new();
        
        public PaymentMethod()
        {
            Id = Guid.NewGuid();
        }

        public PaymentMethod(string key, string title)
        {
            Id = Guid.NewGuid();
            Key = key;
            Title = title;
        }
    }
}