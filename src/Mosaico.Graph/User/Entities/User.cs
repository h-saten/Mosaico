using Mosaico.Domain.Mongodb.Base.Models;

namespace Mosaico.Graph.User.Entities
{
    public class User : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string KangaUserId { get; set; }
        public bool IsAMLVerified { get; set; }
    }
}