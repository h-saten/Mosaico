using System.Threading.Tasks;
using Mosaico.Application.BusinessManagement.CounterProviders;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(UpdateCountersOnCompanyCreated),  "companies:api")]
    [EventTypeFilter(typeof(CompanyCreatedEvent))]
    public class UpdateCountersOnCompanyCreated : EventHandlerBase
    {
        private readonly CompanyCounterProvider _companyCounterProvider;
        private readonly ICountersDispatcher _countersDispatcher;
        private readonly ILogger _logger;

        public UpdateCountersOnCompanyCreated(CompanyCounterProvider projectCounterProvider, ICountersDispatcher countersDispatcher, ILogger logger = null)
        {
            _companyCounterProvider = projectCounterProvider;
            _countersDispatcher = countersDispatcher;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<CompanyCreatedEvent>();
            if (data != null)
            {
                var projectCounters = await _companyCounterProvider.GetCountersAsync(data.CreatedById);
                await _countersDispatcher.DispatchCounterAsync(data.CreatedById, projectCounters);
            }
        }
    }
}