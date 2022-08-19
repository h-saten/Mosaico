using System.Globalization;
using System.Threading.Tasks;
using Mosaico.Application.Statistics.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;

namespace Mosaico.Application.Statistics.EventHandlers
{
    [EventInfo(nameof(UpdateUserCounterKPI), "users:api")]
    [EventTypeFilter(typeof(UserCountUpdated))]
    public class UpdateUserCounterKPI : EventHandlerBase
    {
        private readonly IKPIService _kpiService;

        public UpdateUserCounterKPI(IKPIService kpiService)
        {
            _kpiService = kpiService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<UserCountUpdated>();
            if (data != null)
            {
                await _kpiService.CreateOrUpdateKPIAsync("TOTAL_INVESTORS", data.Count.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}