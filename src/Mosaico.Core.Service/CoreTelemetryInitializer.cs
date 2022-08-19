using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Mosaico.Core.Service
{
    public class CoreTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = "mosaico-core";
                telemetry.Context.Cloud.RoleInstance = "mosaico-core";
            }
        }
    }
}