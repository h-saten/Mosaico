using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class KangaUser : EntityBase
    {
        public string KangaAccountId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string Email { get; set; }
        public bool KycVerified { get; set; }
        
        public KangaUser()
        {
            Id = Guid.NewGuid();
        }
    }
}