using System;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class TeamMemberDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsExpired { get; set; }
    }
}