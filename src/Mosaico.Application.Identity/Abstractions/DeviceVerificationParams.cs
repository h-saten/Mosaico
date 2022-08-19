using Mosaico.Application.Identity.DTOs;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity.Abstractions
{
    public class DeviceVerificationParams
    {
        public ApplicationUser User { get; set; }
        public string IP { get; set; }
        public string AgentInfo { get; set; }
        public string AuthorizationCode { get; set; }
    }
}