using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class Permission : EntityBase
    {
        public Permission()
        {
            Id = Guid.NewGuid();
        }
        
        public virtual List<UserToPermission> Users { get; set; } = new List<UserToPermission>();

        public string Key { get; set; }
    }
}