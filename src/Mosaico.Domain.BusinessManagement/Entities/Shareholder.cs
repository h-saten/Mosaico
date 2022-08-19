using System;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class Shareholder : EntityBase
    {
        public Guid VerificationId { get; set; }
        public virtual Verification Verification { get; set; }
        
        [Encrypted]
        public string FullName { get; set; }
        
        [Encrypted]
        public string Email { get; set; }
        
        public int Share { get; set; }
    }
}