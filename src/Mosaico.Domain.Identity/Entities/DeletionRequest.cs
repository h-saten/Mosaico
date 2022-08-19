using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class DeletionRequest : EntityBase
    {
        
        public virtual ApplicationUser User { get; set;}

        public string JobId { get; set; }

        public string JobName { get; set; }
        public DateTimeOffset DeletionRequestedAt { get; set; }

        public string UserId { get; set; }
             
        public DeletionRequest(string userId, string jobName)
        {
            UserId = userId;
            JobName = jobName;
        }
    }
}