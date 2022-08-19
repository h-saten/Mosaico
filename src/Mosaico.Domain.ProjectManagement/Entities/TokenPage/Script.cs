using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class Script : EntityBase
    {
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
        public string Src { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public bool IsExternal { get; set; }
        public int Order { get; set; }
    }
}