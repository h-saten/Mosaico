using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(UpdateLastLoginOnUserLoggedIn),  "users:id")]
    [EventTypeFilter(typeof(UserLoggedInEvent))]
    public class UpdateLastLoginOnUserLoggedIn : EventHandlerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UpdateLastLoginOnUserLoggedIn(UserManager<ApplicationUser> userManager, ILogger logger = null)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            if (@event != null)
            {
                var eventData = @event.GetData<UserLoggedInEvent>();
                if (eventData != null)
                {
                    _logger?.Verbose($"Starting processing {nameof(UpdateLastLoginOnUserLoggedIn)} for {eventData.Id}");
                    var user = await _userManager.FindByIdAsync(eventData.Id.ToString());
                    if (user != null)
                    {
                        user.LastLogin = DateTime.UtcNow;
                        await _userManager.UpdateAsync(user);
                        _logger?.Verbose($"Successfully updated user's last login");
                    }
                    else
                        _logger?.Verbose($"User {eventData.Id} was not found");
                }
            }
        }
    }
}