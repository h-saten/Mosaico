using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Mosaico.Identity
{
    public class IdentityTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = "mosaico-identity";
                telemetry.Context.Cloud.RoleInstance = "mosaico-identity";
            }
        }
    }
}