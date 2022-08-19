using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ProjectMemberDTO
    {
        public Guid Id { get; set; }
        public bool IsAccepted { get; set; }
        public string Email { get; set; }
        public ProjectRoleDTO Role { get; set; }
        public Guid? UserId { get; set; }
    }
}