using System;
using Mosaico.Application.Identity.DTOs;

namespace Mosaico.Application.Identity.Abstractions
{
    public class DeviceAuthorizationResult
    {
        public bool Success { get; set; }
        public DeviceVerificationTypeDTO? VerificationType { get; set; }
        public DateTimeOffset? CodeExpiryAt { get; set; }
        public string FailureReason { get; set; }
        public bool CanGenerateNewCode { get; set; }
        public bool LastGeneratedCodeStillValid { get; set; }
    }
}