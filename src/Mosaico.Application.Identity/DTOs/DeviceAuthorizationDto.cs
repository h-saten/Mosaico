using System;

namespace Mosaico.Application.Identity.DTOs
{
    public class DeviceAuthorizationDto
    {
        public DeviceVerificationTypeDTO? DeviceVerificationType { get; set; }
        public DateTimeOffset? CodeExpiryAt { get; set; }
        public string FailureReason { get; set; }
        public bool CanGenerateNewCode { get; set; }
        public bool LastGeneratedCodeStillValid { get; set; }
    }
}