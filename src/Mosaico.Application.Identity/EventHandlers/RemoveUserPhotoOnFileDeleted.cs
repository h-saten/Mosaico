using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(RemoveUserPhotoOnFileDeleted),  "documents:id")]
    [EventTypeFilter(typeof(UserPhotoDeletedEvent))]
    public class RemoveUserPhotoOnFileDeleted : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;

        public RemoveUserPhotoOnFileDeleted(IIdentityContext identityContext, ILogger logger = null)
        {
            _identityContext = identityContext;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserPhotoDeletedEvent>();
            if (userEvent != null)
            {
                var id = userEvent.UserId.ToString();
                var user = await _identityContext.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user == null)
                {
                    throw new UserNotFoundException(userEvent.UserId);
                }
                _logger?.Verbose($"User {userEvent.UserId} was found. Attempting to delete photo");
                user.PhotoUrl = string.Empty;
                await _identityContext.SaveChangesAsync();
                _logger?.Verbose($"User photo was successfully removed");
            }
        }
    }
}