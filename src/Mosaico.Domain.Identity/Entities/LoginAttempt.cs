using System;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class LoginAttempt : EntityBase
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTimeOffset LoggedInAt { get; set; }
        public string IP { get; set; }
        public string AgentInfo { get; set; }
        public bool Successful { get; set; }
    }
}