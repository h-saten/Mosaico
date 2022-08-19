using System;

namespace Mosaico.SDK.Identity.Models
{
    public class MosaicoPermission
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        
        public Guid? EntityId { get; set; }
    }
}