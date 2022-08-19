using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class PageTeamMember : EntityBase
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public int Order { get; set; }
        public bool IsHidden { get; set; }
        public string PhotoUrl { get; set; }
        public virtual Page Page { get; set; }
        public Guid PageId { get; set; }
    }
}
