using System.Threading.Tasks;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(RemoveUserPermissionsOnRequested),  "users:id")]
    [EventTypeFilter(typeof(RemoveUserPermissionsRequested))]
    public class RemoveUserPermissionsOnRequested : EventHandlerBase
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ILogger _logger;

        public RemoveUserPermissionsOnRequested(IUserWriteRepository userWriteRepository, ILogger logger = null)
        {
            _userWriteRepository = userWriteRepository;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            if (@event != null)
            {
                var request = @event.GetData<RemoveUserPermissionsRequested>();
                if (request != null)
                {
                    _logger?.Verbose($"Requested to remove permissions for user {request.Id}");
                    await _userWriteRepository.RemoveUserPermissionsAsync(request.Id, request.PermissionsToDelete);
                    _logger?.Verbose($"Successfully removed {request.PermissionsToDelete.Count} permissions");
                }
            }
        }
    }
}