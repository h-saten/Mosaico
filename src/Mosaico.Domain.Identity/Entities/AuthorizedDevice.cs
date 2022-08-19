using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class AuthorizedDevice : EntityBase
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string IP { get; set; }
        public string AgentInfo { get; set; }
    }
}