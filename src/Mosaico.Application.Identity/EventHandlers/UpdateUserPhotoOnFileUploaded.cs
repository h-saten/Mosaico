using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(UpdateUserPhotoOnFileUploaded),  "documents:id")]
    [EventTypeFilter(typeof(UserPhotoUploaded))]
    public class UpdateUserPhotoOnFileUploaded : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;

        public UpdateUserPhotoOnFileUploaded(IIdentityContext projectDbContext, ILogger logger = null)
        {
            _identityContext = projectDbContext;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserPhotoUploaded>();
            if (userEvent != null)
            {
                var id = userEvent.UserId.ToString();
                var user = await _identityContext.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user == null)
                {
                    throw new UserNotFoundException(userEvent.UserId);
                }
                _logger?.Verbose($"User {userEvent.UserId} was found. Attempting to change photo value to {userEvent.PhotoUrl}");
                user.PhotoUrl = userEvent.PhotoUrl;
                await _identityContext.SaveChangesAsync();
                _logger?.Verbose($"User photo was successfully changed");
            }
        }
    }
}