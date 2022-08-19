using System;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class CompanySubscriber : EntityBase
    {
        public string Email { get; set; }
        public string EmailNormalized { get; set; }
        public string UserId { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        
        [Encrypted]
        public string Code { get; set; }
    }
}