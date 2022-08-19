using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.Identity.Abstractions
{
    public interface IDeviceAuthorizationVerifier
    {
        Task<DeviceAuthorizationResult> VerifyAsync(Action<DeviceVerificationParams> verificationParams, CancellationToken cancellationToken);
    }
}