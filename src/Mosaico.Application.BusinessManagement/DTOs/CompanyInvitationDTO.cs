using System;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class CompanyInvitationDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsAccepted { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public bool Expired { get; set; }
        public bool CanEditRole { get; set; }
    }
}