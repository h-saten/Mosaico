using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ProjectInvestorDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
    }
}