using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class UserToPermission : EntityBase
    {
        public UserToPermission()
        {
            Id = Guid.NewGuid();
        }

        public UserToPermission(Guid? entityId, string userId, Guid permissionId)
        {
            EntityId = entityId;
            UserId = userId;
            PermissionId = permissionId;
            Id = Guid.NewGuid();
        }

        public void SetUser(ApplicationUser user)
        {
            User = user;
            UserId = user.Id;
        }

        public void SetPermission(Permission permission)
        {
            Permission = permission;
            PermissionId = permission.Id;
        }
        
        public Guid? EntityId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Guid PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}