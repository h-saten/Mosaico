using System.Threading.Tasks;
using Mosaico.Application.ProjectManagement.CounterProviders;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(UpdateCountersOnProjectCreated),  "projects:api")]
    [EventTypeFilter(typeof(ProjectCreatedEvent))]
    public class UpdateCountersOnProjectCreated : EventHandlerBase
    {
        private readonly ProjectCounterProvider _projectCounterProvider;
        private readonly ICountersDispatcher _countersDispatcher;
        private readonly ILogger _logger;

        public UpdateCountersOnProjectCreated(ProjectCounterProvider projectCounterProvider, ICountersDispatcher countersDispatcher, ILogger logger = null)
        {
            _projectCounterProvider = projectCounterProvider;
            _countersDispatcher = countersDispatcher;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<ProjectCreatedEvent>();
            if (data != null)
            {
                var projectCounters = await _projectCounterProvider.GetCountersAsync(data.CreatedById);
                await _countersDispatcher.DispatchCounterAsync(data.CreatedById, projectCounters);
            }
        }
    }
}