using System;

namespace Mosaico.Application.Identity.DTOs
{
    public class UserPermissionDTO
    {
        public Guid Id { get; set; }
        public Guid? EntityId { get; set; }
        public string Key { get; set; }
    }
}