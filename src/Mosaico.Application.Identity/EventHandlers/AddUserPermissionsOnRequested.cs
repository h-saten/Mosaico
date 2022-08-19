using System.Threading.Tasks;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(AddUserPermissionsOnRequested),  "users:id")]
    [EventTypeFilter(typeof(AddUserPermissionsRequested))]
    public class AddUserPermissionsOnRequested : EventHandlerBase
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ILogger _logger;

        public AddUserPermissionsOnRequested(IUserWriteRepository userWriteRepository, ILogger logger = null)
        {
            _userWriteRepository = userWriteRepository;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            if (@event != null)
            {
                var request = @event.GetData<AddUserPermissionsRequested>();
                if (request != null)
                {
                    _logger?.Verbose($"Attempting to add permissions to user {request.Id}");
                    await _userWriteRepository.AddUserPermissions(request.Id, request.PermissionsToAdd);
                    _logger?.Verbose($"Successfully added {request.PermissionsToAdd.Count} permissions");
                }
            }
        }
    }
}